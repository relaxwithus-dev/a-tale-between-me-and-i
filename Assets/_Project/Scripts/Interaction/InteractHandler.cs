using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Data;
using ATBMI.Enum;
using ATBMI.Entities.Player;
using ATBMI.Inventory;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Interaction
{
    [RequireComponent(typeof(InteractCreator))]
    public class InteractHandler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")]
        [SerializeField] private GameObject optionPanelUI;
        [SerializeField] private TextMeshProUGUI descriptionTextUI;
        [SerializeField] private List<InteractFlag> basicFlags;
        [SerializeField] private Sprite noneSprite;
        
        private InteractCreator _interactCreator;
        public IInteractable Interactable { get; private set; }

        [Header("Reference")]
        [SerializeField] private SimpleScrollSnap scrollSnap;
        [SerializeField] private InteractManager interactManager;
        [SerializeField] private PlayerController playerController;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _interactCreator = GetComponent<InteractCreator>();
        }

        private void Start()
        {
            InitBasicButtons();
            InitScrollSnap();

            optionPanelUI.SetActive(false);
        }
        
        private void Update()
        {
            if (Interactable == null) return;

            HandleNavigation();
            HandleDescription();
        }

        #endregion

        #region Methods
        
        // !- Initialize
        private void InitScrollSnap()
        {
            scrollSnap.Setup();
            scrollSnap.HandleInfiniteScrolling(true);
        }

        private void InitBasicButtons()
        {
            var basicCount = basicFlags.Count;
            for (var i = 0; i < basicCount; i++)
            {
                var flags = basicFlags[i];
                flags.FlagButton.onClick.AddListener(() => OnButtonClicked(flags));
            }
        }
        
        private void InitItemButtons()
        {
            var flagCount = _interactCreator.InventoryFlags.Count;
            var inventoryCount = InventoryManager.Instance.InventoryList.Count;
            int maxCount = Mathf.Max(flagCount, inventoryCount);

            for (int i = 0; i < maxCount; i++)
            {
                var flags = _interactCreator.InventoryFlags[i] as InteractFlag;
                var button = flags.FlagButton;

                if (i < inventoryCount)
                {
                    var itemData = InventoryManager.Instance.GetItemData(i);

                    // Setup listener
                    button.onClick.RemoveAllListeners(); 
                    button.onClick.AddListener(() => OnButtonClicked(flags));
                    
                    SetButtonFlags(flags, flagId: i, itemData.name, itemData);
                    SetButtonData(flags, itemData.ItemSprite, interactable: true);
                }
                else
                {
                    button.onClick.RemoveAllListeners();
                    SetButtonFlags(flags, 0, "");
                    SetButtonData(flags, noneSprite, interactable: false);
                }
            }
        }

        private void SetButtonData(InteractFlag flag, Sprite sprite, bool interactable)
        {
            flag.FlagButton.interactable = interactable;
            flag.FlagIcon.sprite = sprite;
        }

        private void SetButtonFlags(InteractFlag flag, int flagId, string flagName, ItemData data = null)
        {
            flag.SetFlags(flagId, flagName, InteractFlagStatus.Item, data);
        }

        private void OnButtonClicked(InteractFlag flag)
        {
            var (status, data) = flag.GetItemFlags();
            switch (status)
            {
                case InteractFlagStatus.Interaction:
                    Interactable.Interact(interactManager);
                    break;
                case InteractFlagStatus.Item:
                    Interactable.Interact(interactManager, data.ItemId);
                    break;
            }

            StartCoroutine(CloseInteractOption());
        }

        // !- Core
        public void OpenInteractOption(IInteractable interactable)
        {
            // Setup
            InitItemButtons();
            InitScrollSnap();

            // Neccesary
            Interactable = interactable;
            playerController.StopMovement();
            optionPanelUI.SetActive(true);
        }

        private IEnumerator CloseInteractOption()
        {
            Interactable = null;
            optionPanelUI.SetActive(false);
            playerController.StartMovement();

            yield return new WaitForSeconds(0.05f);
            interactManager.IsInteracted = false;
        }

        private void HandleNavigation()
        {
            if (GameInputHandler.Instance.IsNavigateUp)
            {
                scrollSnap.GoToNextPanel();
            }
            else if (GameInputHandler.Instance.IsNavigateDown)
            {
                scrollSnap.GoToPreviousPanel();
            }
            else if (GameInputHandler.Instance.IsTapInteract)
            {
                InvokeInteract();
            }
        }

        private void InvokeInteract()
        {
            var index = scrollSnap.SelectedPanel;
            var button = GetFlags(index)[GetAdjustedIndex(index)].FlagButton;
            button.onClick.Invoke();
        }
        
        private void HandleDescription()
        {
            var index = scrollSnap.SelectedPanel;
            var description = GetFlags(index)[GetAdjustedIndex(index)].FlagName;
            descriptionTextUI.text = description;
        }

        // !- Utilities
        private List<InteractFlag> GetFlags(int index)
        {
            return index < basicFlags.Count ? 
                basicFlags : _interactCreator.InventoryFlags
                                .OfType<InteractFlag>()
                                .ToList();
        }

        private int GetAdjustedIndex(int index)
        {
            return index < basicFlags.Count ? index : index - basicFlags.Count;
        }

        #endregion
    }
}