using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Enum;
using ATBMI.Inventory;
using ATMBI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Handler;

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

        private List<InteractData> _interactContainer;
        public List<BaseInteract> TargetContainer { get; set;}
        public bool IsInteracting { get; set;}

        [Header("References")]
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        private PlayerControllers _playerController;
        private InventoryManager _inventoryManager;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerController = GetComponent<PlayerControllers>();
            _inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        }
        
        private void OnEnable()
        {
            PlayerEventHandler.OnInteract += OnInteractTriggered;
        }
        
        private void OnDisable()
        {
            PlayerEventHandler.OnInteract -= OnInteractTriggered;
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
            var target = TargetContainer[0];
            button.onClick.AddListener(() => SubsInteraction(index, target));
        }
        
        private void SubsInteraction(int index, BaseInteract target)
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
            IsInteracting = false;
            optionsPanelUI.SetActive(false);
            _playerController.StartMovement();
            optionInfoTextUI.text = _interactContainer[0].Description;
            ResetButtons();
        }

        #endregion

        #region Interaction Methods

        private void HandleNavigation()
        {
            if (GameInputHandler.Instance.IsNavigateUp)
            {
                simpleScrollSnap.GoToNextPanel();
            }
            else if (GameInputHandler.Instance.IsNavigateDown)
            {
                simpleScrollSnap.GoToPreviousPanel();
            }
        }
        
        private void HandleInteraction()
        {
            if (GameInputHandler.Instance.IsTapInteract && IsInteracting)
            {
                ExecuteInteraction();
            }
        }

        private void ExecuteInteraction()
        {
            var selectIndex = simpleScrollSnap.SelectedPanel;
            var container = _interactContainer[selectIndex];

            // Removed inventory
            if (selectIndex > 1)
            {
                var collectible = container.Interactable as CollectibleInteract;
                
                if (collectible.TargetId == TargetContainer[0].InteractId)
                {
                    _inventoryManager.CollectibleItem.Remove(collectible.gameObject);
                    _interactContainer.RemoveAt(selectIndex);
                    Destroy(container.Button.transform.parent.gameObject);
                }
            }

            // Execute button
            container.Button.onClick.Invoke();
            if (selectIndex != 1) ExitButton();
        }

        private void HandleInteractDescription()
        {
            var index = simpleScrollSnap.SelectedPanel;

            if (index > _interactContainer.Count - 1) return;
            optionInfoTextUI.text = _interactContainer[index].Description;
        }

        #endregion

    }
}