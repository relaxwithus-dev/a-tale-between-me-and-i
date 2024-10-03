using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Enum;
using ATBMI.Player;
using ATBMI.Inventory;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Interaction
{
    /// <summary>
    /// InteractHandler buat handle interact option
    /// karakter player, tergantung dengan tipe
    /// object yang di-interact.
    /// </summary>
    public class InteractHandler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")]
        [SerializeField] private GameObject optionPanelUI;
        [SerializeField] private GameObject optionContentUI;
        [SerializeField] private TextMeshProUGUI descriptionTextUI;
        [SerializeField] private List<InteractFlag> basicFlags;
        [SerializeField] private List<InteractFlag> itemFlags;

        public IInteractable Interactable { get; private set; }

        [Header("Assets")]
        [SerializeField] private InteractFlag contentPrefabs;
        [SerializeField] private Sprite noneSprite;

        [Header("Reference")]
        [SerializeField] private SimpleScrollSnap scrollSnap;
        [SerializeField] private InteractManager interactManager;
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private PlayerController playerController;

        #endregion

        #region MonoBehaviour Callbacks

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
                var button = flags.FlagButton;
                button.onClick.AddListener(() => OnButtonClicked(flags));
            }
        }

        // TODO: Drop instantiate flag, diganti sama UpdateInventory method
        private void InitItemButtons()
        {
            var itemButtonCount = itemFlags.Count;
            var inventoryCount = inventoryManager.Item.Count;
            int maxCount = Mathf.Max(itemButtonCount, inventoryCount);

            for (int i = 0; i < maxCount; i++)
            {
                Button button;
                if (i > itemButtonCount - 1)
                {
                    // Instantiate new button
                    var newBtn = Instantiate(contentPrefabs, optionContentUI.transform, worldPositionStays: false);
                    button = newBtn.FlagButton;
                    itemFlags.Add(newBtn);
                }
                else
                {
                    button = itemFlags[i].FlagButton;
                }

                var flags = itemFlags[i];
                if (i < inventoryCount)
                {
                    var itemData = inventoryManager.Item[i].GetItemData();

                    // Setup listener
                    button.onClick.RemoveAllListeners(); 
                    button.onClick.AddListener(() => OnButtonClicked(flags));
                    
                    SetButtonFlags(flags, flagId: i, itemData.name, itemData.id);
                    SetButtonData(flags, itemData.sprite, interactable: true);
                }
                else
                {
                    button.onClick.RemoveAllListeners();
                    SetButtonFlags(flags, 0, "", 0);
                    SetButtonData(flags, noneSprite, interactable: false);
                }
            }
        }

        private void SetButtonData(InteractFlag flag, Sprite sprite, bool interactable)
        {
            flag.FlagButton.interactable = interactable;
            flag.FlagIcon.sprite = sprite;
        }

        private void SetButtonFlags(InteractFlag flag, int flagId, string flagName, int itemId)
        {
            flag.SetFlags(flagId, flagName, InteractFlagStatus.Item, itemId);
        }

        private void RemoveButton(InteractFlag flag)
        {
            itemFlags.Remove(flag);
            Destroy(flag.gameObject);
        }

        private void OnButtonClicked(InteractFlag flag)
        {
            var (status, itemId) = flag.GetItemFlags();
            switch (status)
            {
                case InteractFlagStatus.Interaction:
                    Execute();
                    break;
                case InteractFlagStatus.Item:
                    Execute(flag, itemId);
                    break;
            }

            StartCoroutine(CloseInteractOption());
        }

        private void Execute()
        {
            Interactable.Interact(interactManager);
        }

        // TODO: Drop status, diganti sama UpdateInventory method
        private void Execute(InteractFlag flag, int itemId)
        {
            Interactable.Interact(interactManager, itemId);
            if (Interactable.Status())
            {
                if (itemFlags.Count > 3)
                    RemoveButton(flag);
                else
                    SetButtonData(flag, noneSprite, interactable: false);
                
                inventoryManager.Item.RemoveAt(flag.FlagId);
            }
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
            return index < basicFlags.Count ? basicFlags : itemFlags;
        }

        private int GetAdjustedIndex(int index)
        {
            return index < basicFlags.Count ? index : index - basicFlags.Count;
        }

        #endregion
    }
}
