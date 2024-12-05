using ATBMI.Gameplay.Handler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System;
using ATBMI.Data;

namespace ATBMI.Inventory
{
    // TODO: Pake class ini buat handle UI Inventory
    [RequireComponent(typeof(InventoryCreator))]
    public class InventoryHandler : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject menuUI;
        [SerializeField] private Sprite noneImageUI;
        [SerializeField] private Image itemImageUI;
        [SerializeField] private TextMeshProUGUI itemNameTextUI;
        [SerializeField] private TextMeshProUGUI itemDescriptionTextUI;

        // [Header("Reference")]
        private InventoryCreator _inventoryCreator;
        private bool isMenuActive;
        private int selectedIndex;
        private int flagCount;

        private void Awake()
        {
            _inventoryCreator = GetComponent<InventoryCreator>();

            isMenuActive = false;
            selectedIndex = -1; // not selected
        }

        private void Update()
        {
            // TODO: change with new input system
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isMenuActive)
                {
                    OpenInventoryMenu();
                }
                else
                {
                    CloseInventoryMenu();
                }
            }

            if (!isMenuActive) return;

            HandleNavigation();
        }

        private void OpenInventoryMenu()
        {
            menuUI.SetActive(true);
            isMenuActive = true;

            InitInventoryItemUI();

            // at least there is one inventory, set the selected gameobject with index 0
            if (_inventoryCreator.InventoryFlags.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(_inventoryCreator.InventoryFlags[0].gameObject);
                selectedIndex = 0;

                SetDescription();
            }
            else
            {
                SetDescriptionToNull();

                selectedIndex = -1;
            }
        }

        private void InitInventoryItemUI()
        {
            flagCount = _inventoryCreator.InventoryFlags.Count;

            for (int i = 0; i < flagCount; i++)
            {
                var flags = _inventoryCreator.InventoryFlags[i] as InventoryFlag;
                // var button = flags.FlagButton;

                var item = InventoryManager.Instance.InventoryList[i];
                var itemData = InventoryManager.Instance.GetItemData(item.ItemId);

                SetInventoryFlag(flags, flagId: i, itemData.name, itemData);
                SetInventoryData(flags, itemData.ItemSprite, itemData.ItemName);
            }
        }

        private void SetInventoryFlag(InventoryFlag flag, int flagId, string flagName, ItemData data)
        {
            flag.SetFlags(flagId, flagName, data);
        }

        private void SetInventoryData(InventoryFlag flag, Sprite sprite, string flagName)
        {
            flag.FlagImage.sprite = sprite;
            flag.FlagNameText.text = flagName;
        }

        private void CloseInventoryMenu()
        {
            menuUI.SetActive(false);

            EventSystem.current.SetSelectedGameObject(null);

            selectedIndex = -1;

            isMenuActive = false;
        }

        private void HandleNavigation()
        {
            if (GameInputHandler.Instance.IsNavigateUp)
            {
                Navigate(1);
            }
            else if (GameInputHandler.Instance.IsNavigateDown)
            {
                Navigate(-1);
            }
        }

        private void Navigate(int direction)
        {
            // Update index with wrapping
            selectedIndex = (selectedIndex + direction + _inventoryCreator.InventoryFlags.Count) % _inventoryCreator.InventoryFlags.Count;

            // Update description
            SetDescription();
        }

        private void SetDescription()
        {
            var selectedFlag = _inventoryCreator.InventoryFlags[selectedIndex] as InventoryFlag;
            var selectedFlagData = selectedFlag.ItemData;

            itemImageUI.sprite = selectedFlagData.ItemSprite;
            itemNameTextUI.text = selectedFlagData.ItemName;
            itemDescriptionTextUI.text = selectedFlagData.ItemDescription;
        }

        private void SetDescriptionToNull()
        {
            itemImageUI.sprite = noneImageUI;
            itemNameTextUI.text = "???";
            itemDescriptionTextUI.text = "???";
        }
    }
}