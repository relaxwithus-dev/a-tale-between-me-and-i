using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Enum;
using ATBMI.Entities.Player;
using ATBMI.Inventory;

namespace ATBMI.Interaction
{
    public class InteractOptions : MonoBehaviour
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject optionsPanelUI;
        [SerializeField] private GameObject interactOptionsUI;
        [SerializeField] private TextMeshProUGUI optionInfoTextUI;

        private List<Button> _optionButtons;
        private int _collectibleIndex;

        [Header("References")]
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;
        private InteractArea _interactArea;
        private PlayerController _playerController;
        private PlayerInputHandler _playerInputHandler;
        private InteractEventHandler _interactEventHandler;
        private InventoryManager _inventoryManager;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _interactArea = GetComponent<InteractArea>();
            _inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        }

        private void OnEnable()
        {
            // Inject Variables
            _playerInputHandler = _interactArea.PlayerInputHandler;
            _interactEventHandler = _interactArea.InteractEventHandler;
            _playerController = _interactArea.PlayerController;

            // Event
            _interactEventHandler.OnOpenInteract += OnInteractTriggered;
        }
        
        private void OnDisable()
        {
            // Event
            _interactEventHandler.OnOpenInteract -= OnInteractTriggered;
        }

        private void Start()
        {
            optionsPanelUI.SetActive(false);
            _collectibleIndex = 0;
        }

        private void Update()
        {
            if (!_interactArea.IsInteracting) return;

            HandleNavigation();
            HandleInteraction();
        }

        #endregion

        #region Methods

        // !-- Core Functionality
        private void OnInteractTriggered()
        {
            simpleScrollSnap.Setup();
            InitilizeButtons();
            optionsPanelUI.SetActive(true);
        }

        // !-- Button Fields
        private void InitilizeButtons()
        {
            var optionsCount = interactOptionsUI.transform.childCount;
            _optionButtons = new List<Button>(optionsCount);
            for (var i = 0; i < optionsCount; i++)
            {
                var buttonParent = interactOptionsUI.transform.GetChild(i);
                var optionsButton = buttonParent.GetComponentInChildren<Button>();
                if (optionsButton.TryGetComponent(out InteractButton interact))
                {
                    _optionButtons.Add(optionsButton);
                    SetupButtonId(interact, i);
                    SetupButtonListener(optionsButton, interact);
                }
            }
        }

        private void ResetButtons()
        {
            foreach (var button in _optionButtons)
            {
                button.onClick.RemoveAllListeners();
            }
            _optionButtons.Clear();
            _collectibleIndex = 0;
        }

        private void SetupButtonId(InteractButton interact, int id)
        {
            switch (interact.InteractType)
            {
                case InteractType.Talks:
                case InteractType.Close:
                    interact.ButtonId = id;
                    break;
                case InteractType.Item:
                    var inventory = _inventoryManager.CollectibleItem;
                    var itemId = inventory[_collectibleIndex].GetComponent<BaseInteract>().InteractId;
                    interact.ButtonId = itemId;
                    _collectibleIndex++;
                    break;
            }
        }
        
        private void SetupButtonListener(Button button, InteractButton interact)
        {
            var target = _interactArea.InteractTarget;
            button.onClick.AddListener(() => ExecuteInteraction(interact, target));
        }

        private void ExecuteInteraction(InteractButton interact, BaseInteract target)
        {
            switch (interact.InteractType)
            {
                case InteractType.Talks:
                case InteractType.Static_Item:
                    target.Interact();
                    break;
                case InteractType.Item:
                    var inventory = _inventoryManager.CollectibleItem;
                    foreach (var item in inventory)
                    {
                        var collectible = item.GetComponent<BaseInteract>();
                        if (interact.ButtonId != collectible.InteractId) continue;
                        collectible.InteractCollectible(target);
                    }
                    break;
                case InteractType.Close:
                    ExitButton();
                    break;
            }
        }
        
        // !-- Controller Fields
        private void HandleNavigation()
        {
            if (_playerInputHandler.IsPressNavigate(NavigateState.Up))
            {
                simpleScrollSnap.GoToNextPanel();
            }
            else if (_playerInputHandler.IsPressNavigate(NavigateState.Down))
            {
                simpleScrollSnap.GoToPreviousPanel();
            }
        }

        private void HandleInteraction()
        {
            if (_playerInputHandler.IsPressInteract())
            {
                ExecuteInteraction();
            }
        }

        private void ExecuteInteraction()
        {
            var selectIndex = simpleScrollSnap.SelectedPanel;
            if (_optionButtons[selectIndex].TryGetComponent(out InteractButton button))
            {
                _optionButtons[selectIndex].onClick.Invoke();
                if (button.InteractType != InteractType.Item) return;

                var optionParent = _optionButtons[selectIndex].transform.parent;
                _optionButtons.RemoveAt(selectIndex);
                Destroy(optionParent.gameObject);
                ExitButton();
            }
        }

        // !-- Button Methods
        private void ExitButton()
        {
            optionsPanelUI.SetActive(false);
            _interactArea.IsInteracting = false;

            _playerController.StartMovement();
            simpleScrollSnap.Setup();
            ResetButtons();
        }

        #endregion

    }
}
