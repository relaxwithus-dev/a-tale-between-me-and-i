using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Enum;
using ATBMI.Inventory;
using ATBMI.Entities.Player;

namespace ATBMI.Interaction
{
    public class InteractController : MonoBehaviour
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject optionsPanelUI;
        [SerializeField] private GameObject interactOptionsUI;
        [SerializeField] private TextMeshProUGUI optionInfoTextUI;
        [SerializeField] private GameObject contentPrefabs;

        [SerializeField] private List<InteractData> _interactContainer;
        [field: SerializeField] public List<BaseInteract> TargetContainer { get; set;}
        public bool IsInteracting { get; set;}

        [Header("References")]
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        private PlayerController _playerController;
        private PlayerInputHandler _playerInputHandler;
        private InventoryManager _inventoryManager;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerController = player.GetComponent<PlayerController>();
            _playerInputHandler = player.GetComponentInChildren<PlayerInputHandler>();
            _inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        }

        private void OnEnable()
        {
            InteractEventHandler.OnOpenInteract += OnInteractTriggered;
        }
        
        private void OnDisable()
        {
            InteractEventHandler.OnOpenInteract -= OnInteractTriggered;
        }

        private void Start()
        {
            optionsPanelUI.SetActive(false);
            TargetContainer = new List<BaseInteract>();
        }

        private void Update()
        {
            if (!IsInteracting) return;

            HandleNavigation();
            HandleInteraction();
            HandleInteractDescription();
        }

        #endregion

        #region Button Field Methods

        // !-- Core Functionality
        private void OnInteractTriggered()
        {
            InitializeContent();
            InitializeButtons();
            SetupScrollSnap();
            optionsPanelUI.SetActive(true);
        }

        private void SetupScrollSnap()
        {
            simpleScrollSnap.Setup();
            simpleScrollSnap.HandleInfiniteScrolling(true);
        }

        private void InitializeContent()
        {
            // TODO: Drop logic buat init content disini
            var optionsCount = interactOptionsUI.transform.childCount - 2;
            var inventoryCount = _inventoryManager.CollectibleItem.Count;

            if (inventoryCount > optionsCount)
            {
                var countGap = Mathf.Abs(inventoryCount - optionsCount);
                for (var i = 0; i < countGap; i++)
                {
                    Instantiate(contentPrefabs, interactOptionsUI.transform, worldPositionStays: false);
                }
            }
        }
        
        private void InitializeButtons()
        {
            var optionsCount = interactOptionsUI.transform.childCount;
            _interactContainer = new List<InteractData>(optionsCount);

            for (var i = 0; i < optionsCount; i++)
            {
                var interactButton = interactOptionsUI.transform.GetChild(i).GetComponentInChildren<Button>();
                var interactDataFactory = new InteractDataFactory(_inventoryManager);
                var interactData = interactDataFactory.CreateInteractData(i, interactButton);
                
                _interactContainer.Add(interactData);

                SetupButtonImage(i, interactButton);
                SetupButtonListener(i, interactButton);
            }
        }

        private void ResetButtons()
        {
            foreach (var interact in _interactContainer)
            {
                interact.Button.onClick.RemoveAllListeners();
            }
            _interactContainer.Clear();
        }

        private void SetupButtonImage(int index, Button button)
        {
            if (index < 2) return;

            var collectibleItem = _inventoryManager.CollectibleItem[index - 2];
            var butonImage = button.transform.GetChild(0).GetComponent<Image>();
            butonImage.sprite = collectibleItem.GetComponent<SpriteRenderer>().sprite;
        }
        
        private void SetupButtonListener(int index, Button button)
        {
            // Ambil target pertama
            var target = TargetContainer[0];
            button.onClick.AddListener(() => ExecuteInteraction(index, target));
        }

        private void ExecuteInteraction(int index, BaseInteract target)
        {
            switch (index)
            {
                case 0:
                    target.Interact();
                    break;
                case 1:
                    ExitButton();
                    break;
                default:
                    _interactContainer[index].Interactable.InteractCollectible(target);
                    break;
            }
        }

        private void ExitButton()
        {
            optionsPanelUI.SetActive(false);
            IsInteracting = false;

            _playerController.StartMovement();
            ResetButtons();
        }

        #endregion

        #region Interaction Methods

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
            var container = _interactContainer[selectIndex];

            container.Button.onClick.Invoke();
            if (selectIndex >= 2)
            {
                var inventory = _inventoryManager.CollectibleItem;
                foreach (var item in inventory)
                {
                    var collectible = item.GetComponent<CollectibleInteract>();
                    if (collectible.TargetId == TargetContainer[0].InteractId)
                    {
                        inventory.Remove(item);
                        _interactContainer.RemoveAt(selectIndex);
                        Destroy(container.Button.transform.parent.gameObject);
                    }
                    break;
                }
            }
            ExitButton();
        }

        private void HandleInteractDescription()
        {
            if (!IsInteracting) return;
        
            var index = simpleScrollSnap.SelectedPanel;
            var optionInfo = _interactContainer[index].Description;
            optionInfoTextUI.text = optionInfo;
        }

        #endregion

    }
}