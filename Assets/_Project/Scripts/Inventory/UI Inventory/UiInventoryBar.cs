using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Enum;
using ATBMI.Gameplay.Event;

namespace ATBMI.Inventory
{
    public class UiInventoryBar<T> : MonoBehaviour where T : class
    {
        public InventoryLocation inventoryLocation;
        public GameObject playerInventorySlotPrefab;
        public GameObject uiInventorySlotPrefab;
        private readonly List<T> inventorySlots = new();
        private IInventorySlotFactory<T> inventorySlotFactory;

        private void OnEnable()
        {
            PlayerEvents.OnUpdateInventory += UpdateInventory;
        }

        private void OnDisable()
        {
            PlayerEvents.OnUpdateInventory -= UpdateInventory;
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
            int initialInventorySize = InventoryManager.Instance.GetStartingItemCount();
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
                    ItemData itemDetails = InventoryManager.Instance.GetItemDetails(inventoryList[i].itemId);
                    if (itemDetails != null)
                    {
                        if (inventorySlots[i] is UiPlayerInventorySlot playerSlot)
                        {
                            playerSlot.itemDetails = itemDetails;
                            playerSlot.image.sprite = itemDetails.ItemSprite;
                            playerSlot.itemName = itemDetails.ItemName;
                        }
                        else if (inventorySlots[i] is UiInventorySlot uiSlot)
                        {
                            uiSlot.itemDetails = itemDetails;
                            uiSlot.image.sprite = itemDetails.ItemSprite;
                            uiSlot.itemName = itemDetails.ItemName;
                            uiSlot.itemDescription = itemDetails.ItemDescription;
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
