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
    public class InteractOptions : MonoBehaviour
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

        private Dictionary<int, Dictionary<int, Button>> _interactContainers;

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
            _interactContainers = new Dictionary<int, Dictionary<int, Button>>(optionsCount);

            for (var i = 0; i < optionsCount; i++)
            {
                var interactButton = interactOptionsUI.transform.GetChild(i).GetComponentInChildren<Button>();
                var interactKey = GetInteractKey(i);

                _interactContainers[i] = new Dictionary<int, Button>
                {
                    { interactKey, interactButton }
                };
                
                SetupButtonImage(i, interactButton);
                SetupButtonListener(i, interactKey, interactButton);
            }
        }

        private void ResetButtons()
        {
            foreach (var IndexValuePair in _interactContainers)
            {
                foreach (var KeyValuePair in IndexValuePair.Value)
                {
                    var button = KeyValuePair.Value;
                    button.onClick.RemoveAllListeners();
                }
            }
            _interactContainers.Clear();
        }

        private void SetupButtonImage(int index, Button button)
        {
            if (index < 2) return;

            var collectibleItem = _inventoryManager.CollectibleItem[index - 2];
            var butonImage = button.transform.GetChild(0).GetComponent<Image>();
            butonImage.sprite = collectibleItem.GetComponent<SpriteRenderer>().sprite;
        }
        
        private void SetupButtonListener(int index, int key, Button button)
        {
            var target = _interactArea.InteractTarget;
            button.onClick.AddListener(() => ExecuteInteraction(target, index, key));
        }

        private void ExecuteInteraction(BaseInteract target, int index, int key)
        {
            switch (key)
            {
                case 0:
                    target.Interact();
                    break;
                case 1:
                    ExitButton();
                    break;
                default:
                    var inventory = _inventoryManager.CollectibleItem;
                    foreach (var item in inventory)
                    {
                        var collectible = item.GetComponent<BaseInteract>();
                        if (_interactContainers[index].ContainsKey(collectible.InteractId))
                        {
                            collectible.InteractCollectible(target);
                        }
                    }
                    break;
            }
        }

        // !-- Helpers
        private int GetInteractKey(int index)
        {
            if (index < 2)
            {
                return index;
            }
            else
            {
                var inventoryItem = _inventoryManager.CollectibleItem[index - 2].GetComponent<BaseInteract>();
                return inventoryItem.InteractId;
            }
        }

        #endregion

        #region Interaction Controller Methods

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
            var container = _interactContainers[selectIndex];

            if (selectIndex < 2)
            {
                container[selectIndex].onClick.Invoke();
            }
            else
            {
                var inventory = _inventoryManager.CollectibleItem;
                foreach (var item in inventory)
                {
                    var collectible = item.GetComponent<CollectibleInteract>();
                    if (container.TryGetValue(collectible.InteractId, out var interactOption))
                    {
                        interactOption.onClick.Invoke();
                        if (collectible.TargetId == _interactArea.InteractTarget.InteractId)
                        {
                            inventory.Remove(item);
                            _interactContainers.Remove(selectIndex);
                            Destroy(interactOption.transform.parent.gameObject);
                        }
                        break;
                    }
                }
            }
            ExitButton();
        }

        private void ExitButton()
        {
            optionsPanelUI.SetActive(false);
            _interactArea.IsInteracting = false;

            _playerController.StartMovement();
            ResetButtons();
        }

        #endregion

    }
}