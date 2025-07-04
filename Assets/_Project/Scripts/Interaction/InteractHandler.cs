using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Data;
using ATBMI.Enum;
using ATBMI.Inventory;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

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
        
        private IInteractable _interactable;
        private InteractCreator _interactCreator;

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
            if (_interactable == null) return;

            HandleNavigation();
            HandleDescription();
        }
        
        #endregion

        #region Methods
        
        // Initialize
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
                    var item = InventoryManager.Instance.InventoryList[i];
                    var itemData = InventoryManager.Instance.GetItemData(item.ItemId);

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
                    _interactable.Interact();
                    break;
                case InteractFlagStatus.Item:
                    _interactable.Interact(data.ItemId);
                    break;
            }
            
            StartCoroutine(CloseInteractOption());
        }
        
        // Core
        public void OpenInteractOption(IInteractable interactable)
        {
            InitItemButtons();
            InitScrollSnap();
            
            _interactable = interactable;
            optionPanelUI.SetActive(true);
        }
        
        private IEnumerator CloseInteractOption()
        {
            _interactable = null;
            optionPanelUI.SetActive(false);
            
            yield return new WaitForSeconds(0.05f);
            InteractEvent.InteractedEvent(interact: false, playerController);
        }
        
        private void HandleNavigation()
        {
            if (GameInputHandler.Instance.IsArrowRight)
            {
                scrollSnap.GoToNextPanel();
            }
            else if (GameInputHandler.Instance.IsArrowLeft)
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
            GetFlags(index)[GetAdjustedIndex(index)].FlagButton.onClick.Invoke();
        }
        
        private void HandleDescription()
        {
            var index = scrollSnap.SelectedPanel;
            descriptionTextUI.text = GetFlags(index)[GetAdjustedIndex(index)].FlagName;
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