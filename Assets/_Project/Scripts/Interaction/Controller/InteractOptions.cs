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
using System.Linq;

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
            simpleScrollSnap.Setup();
            InitializeButtons();
            optionsPanelUI.SetActive(true);
        }

        private void InitializeContent()
        {
            // TODO: Drop logic buat init content disini
            var optionsCount = interactOptionsUI.transform.childCount - 2;
            var inventoryCount = _inventoryManager.CollectibleItem.Count;

            if (optionsCount == inventoryCount) return;
            var countGap = Mathf.Abs(inventoryCount - optionsCount);
            var isExcessive = inventoryCount > optionsCount;
            Debug.LogWarning(countGap);
            
            for (var i = 0; i < countGap; i++)
            {
                if (isExcessive)
                {
                    Instantiate(contentPrefabs, optionsPanelUI.transform, worldPositionStays: false);
                }
                else
                {
                    var interactObject = interactOptionsUI.transform.GetChild(i);
                    Destroy(interactObject.gameObject);
                }
            }
        }

        private void InitializeButtons()
        {
            var optionsCount = interactOptionsUI.transform.childCount;
            _interactContainers = new Dictionary<int, Dictionary<int, Button>>();
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
                            Debug.LogWarning($"interact collectible");
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
                var inventory = _inventoryManager.CollectibleItem[index - 2];
                var item = inventory.GetComponent<BaseInteract>();
                return item.InteractId;
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
                var optionIndex = 0;
                var inventory = _inventoryManager.CollectibleItem;
                foreach (var item in inventory)
                {
                    var collectible = item.GetComponent<BaseInteract>();
                    if (container.ContainsKey(collectible.InteractId))
                    {
                        container[collectible.InteractId].onClick.Invoke();
                        optionIndex = collectible.InteractId;
                        inventory.Remove(item);
                        break;
                    }
                }
                var optionParent = container[optionIndex].transform.parent;
                _interactContainers.Remove(selectIndex);
                Destroy(optionParent.gameObject);
            }
            
            ExitButton();
        }

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
