using ATBMI.Gameplay.Handler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ATBMI.Data;
using ATBMI.Gameplay.Event;
using UnityEngine.InputSystem;

namespace ATBMI.Inventory
{
    // TODO: Pake class ini buat handle UI Inventory
    [RequireComponent(typeof(InventoryCreator))]
    public class InventoryHandler : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Sprite noneImageUI;
        [SerializeField] private Image itemImageUI;
        [SerializeField] private TextMeshProUGUI itemNameTextUI;
        [SerializeField] private TextMeshProUGUI itemDescriptionTextUI;

        [Header("Rect Transforms")]
        [SerializeField] private RectTransform scrollRectTransform;
        [SerializeField] private RectTransform contentRectTransform;

        [Header("Input Actions")]
        [SerializeField] private InputActionReference navigateAction; // This is a Vector2 input


        // [Header("Reference")]
        private InventoryCreator _inventoryCreator;
        private int selectedIndex;
        private int flagCount;

        private void Awake()
        {
            _inventoryCreator = GetComponent<InventoryCreator>();

            selectedIndex = -1; // not selected
        }

        private void OnEnable()
        {
            UIEvents.OnSelectTabInventory += OpenInventoryMenu;
            UIEvents.OnDeselectTabInventory += CloseInventoryMenu;

            navigateAction.action.performed += OnNavigate;
        }

        private void OnDisable()
        {
            UIEvents.OnSelectTabInventory -= OpenInventoryMenu;
            UIEvents.OnDeselectTabInventory -= CloseInventoryMenu;

            navigateAction.action.performed -= OnNavigate;
        }

        private void OpenInventoryMenu()
        {
            InitInventoryItemUI();
            // DebugInventoryFlags();

            if (_inventoryCreator.InventoryFlags.Count > 0)
            {
                selectedIndex = 0;
                (_inventoryCreator.InventoryFlags[selectedIndex] as InventoryFlag)?.Highlight(true); // Highlight first item

                SetDescription();
            }
            else
            {
                SetDescriptionToNull();
                selectedIndex = -1;
            }
        }

        private void CloseInventoryMenu()
        {
            if (selectedIndex != -1)
            {
                (_inventoryCreator.InventoryFlags[selectedIndex] as InventoryFlag)?.Highlight(false);
            }

            selectedIndex = -1;
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

        private void OnNavigate(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();

            if (input.y > 0.5f) // Up
            {
                Navigate(-1);
            }
            else if (input.y < -0.5f) // Down
            {
                Navigate(1);
            }
        }

        private void Navigate(int direction)
        {
            if (_inventoryCreator.InventoryFlags.Count == 0) return;

            // Remove highlight from previous selection
            if (selectedIndex >= 0 && selectedIndex < _inventoryCreator.InventoryFlags.Count)
            {
                (_inventoryCreator.InventoryFlags[selectedIndex] as InventoryFlag)?.Highlight(false);
            }

            // Update index with wrapping
            selectedIndex = (selectedIndex + direction + _inventoryCreator.InventoryFlags.Count) % _inventoryCreator.InventoryFlags.Count;

            // Highlight the new selected element
            var selectedFlag = _inventoryCreator.InventoryFlags[selectedIndex] as InventoryFlag;
            selectedFlag?.Highlight(true);

            // Update description
            SetDescription();

            // Update scrolling, pass RectTransform of selected item
            UpdateScrolling(selectedFlag.transform as RectTransform);
        }

        private void UpdateScrolling(RectTransform buttonRectTransform)
        {
            // calculate the min and max for the selected button
            float buttonYMin = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
            float buttonYMax = buttonYMin + buttonRectTransform.rect.height;

            // calculate the min and max for the content area
            float contentYMin = contentRectTransform.anchoredPosition.y;
            float contentYMax = contentYMin + scrollRectTransform.rect.height;

            // handle scrolling down
            if (buttonYMax > contentYMax)
            {
                contentRectTransform.anchoredPosition = new Vector2(
                    contentRectTransform.anchoredPosition.x,
                    buttonYMax - scrollRectTransform.rect.height
                );
            }
            // handle scrolling up
            else if (buttonYMin < contentYMin)
            {
                contentRectTransform.anchoredPosition = new Vector2(
                    contentRectTransform.anchoredPosition.x,
                    buttonYMin
                );
            }
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

        private void DebugInventoryFlags()
        {
            Debug.Log($"Total Inventory Flags: {_inventoryCreator.InventoryFlags.Count}");

            for (int i = 0; i < _inventoryCreator.InventoryFlags.Count; i++)
            {
                var flag = _inventoryCreator.InventoryFlags[i] as InventoryFlag;
                Debug.Log($"Flag {i}: Name = {flag.FlagNameText.text}, GameObject = {flag.gameObject.name}");
            }
        }

    }
}