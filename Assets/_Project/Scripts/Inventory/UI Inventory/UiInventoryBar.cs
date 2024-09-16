using System.Collections.Generic;
using ATBMI.Enum;
using ATBMI.Inventory;
using ATMBI.Gameplay.Event;
using UnityEngine;

namespace ATBMI.Inventory
{
    public class UiInventoryBar<T> : MonoBehaviour where T : class
    {
        public InventoryLocation inventoryLocation;
        public GameObject playerInventorySlotPrefab;
        public GameObject uiInventorySlotPrefab;
        private List<T> inventorySlots = new List<T>();
        private IInventorySlotFactory<T> inventorySlotFactory;

        private void OnEnable()
        {
            PlayerEventHandler.OnUpdateInventory += UpdateInventory;
        }

        private void OnDisable()
        {
            PlayerEventHandler.OnUpdateInventory -= UpdateInventory;
        }

        private void Start()
        {
            // Initialize the factory based on the inventory type
            switch (inventoryLocation)
            {
                case InventoryLocation.PlayerInventory:
                    inventorySlotFactory = new UiPlayerInventorySlotFactory(playerInventorySlotPrefab, transform) as IInventorySlotFactory<T>;
                    break;
                case InventoryLocation.UIInventory:
                    inventorySlotFactory = new UiInventorySlotFactory(uiInventorySlotPrefab, transform) as IInventorySlotFactory<T>;
                    break;
            }

            // Initialize inventory slots
            int initialInventorySize = NewInventoryManager.Instance.GetStartingItemCount();
            for (int i = 0; i < initialInventorySize; i++)
            {
                CreateInventorySlot();
            }
        }

        private void UpdateInventory(List<InventoryItem> inventoryList)
        {
            // Clear existing inventory slots
            foreach (var slot in inventorySlots)
            {
                if (slot is UiPlayerInventorySlot playerSlot)
                {
                    playerSlot.itemDetails = null;
                    playerSlot.image.sprite = null;
                    playerSlot.itemName = "";
                }
                else if (slot is UiInventorySlot uiSlot)
                {
                    uiSlot.itemDetails = null;
                    uiSlot.image.sprite = null;
                    uiSlot.itemName = "";
                    uiSlot.itemDescription = "";
                }
            }

            // Update inventory slots with new items
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (i < inventoryList.Count)
                {
                    SO_ItemDetails itemDetails = NewInventoryManager.Instance.GetItemDetails(inventoryList[i].itemId);
                    if (itemDetails != null)
                    {
                        if (inventorySlots[i] is UiPlayerInventorySlot playerSlot)
                        {
                            playerSlot.itemDetails = itemDetails;
                            playerSlot.image.sprite = itemDetails.itemSprite;
                            playerSlot.itemName = itemDetails.itemName;
                        }
                        else if (inventorySlots[i] is UiInventorySlot uiSlot)
                        {
                            uiSlot.itemDetails = itemDetails;
                            uiSlot.image.sprite = itemDetails.itemSprite;
                            uiSlot.itemName = itemDetails.itemName;
                            uiSlot.itemDescription = itemDetails.itemDescription;
                        }
                    }
                }
            }

            // Create or destroy inventory slots as needed
            if (inventoryList.Count > inventorySlots.Count)
            {
                for (int i = inventorySlots.Count; i < inventoryList.Count; i++)
                {
                    CreateInventorySlot();
                }
            }
            else if (inventoryList.Count < inventorySlots.Count)
            {
                for (int i = inventorySlots.Count - 1; i >= inventoryList.Count; i--)
                {
                    DestroyInventorySlot(i);
                }
            }
        }

        private void CreateInventorySlot()
        {
            T slot = inventorySlotFactory.CreateInventorySlot();
            inventorySlots.Add(slot);
        }

        private void DestroyInventorySlot(int index)
        {
            if (inventorySlots[index] is MonoBehaviour monoBehaviour)
            {
                Destroy(monoBehaviour.gameObject);
            }
            inventorySlots.RemoveAt(index);
        }
    }
}
