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
using Unity.VisualScripting.Dependencies.NCalc;

namespace ATBMI.Interaction
{
    public class InteractController : MonoBehaviour
    {
        #region Fields & Property

        /*
         ! Note Interact Options Mechanic
         * 1. Bikin semacem options/interact container
         * buat nampung data interact talks, exit, dan item inventory
         * 2. Button id bisa pake dictionary biar lebih irit dan
         * aman buat di maintain
         * 3. Logic init buttons perlu ditambah dan dioptimisasi
         * 4. Bikin instantiate button jika jumlah button ga sesuai sama
         * jumlah item inven + 2 (talks, exit)
         */
         
        [Header("UI")]
        [SerializeField] private GameObject optionsPanelUI;
        [SerializeField] private GameObject interactOptionsUI;
        [SerializeField] private TextMeshProUGUI optionInfoTextUI;
        [SerializeField] private GameObject contentPrefabs;

        private List<InteractData> _interactContainer;

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
        }

        private void Update()
        {
            if (!_interactArea.IsInteracting) return;

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
            var target = _interactArea.InteractTarget;
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
            _interactArea.IsInteracting = false;

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
                    if (collectible.TargetId == _interactArea.InteractTarget.InteractId)
                    {
                        Debug.Log("remove!");
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
            if (!_interactArea.IsInteracting) return;
        
            var index = simpleScrollSnap.SelectedPanel;
            var optionInfo = _interactContainer[index].Description;
            optionInfoTextUI.text = optionInfo;
        }

        #endregion

    }
}