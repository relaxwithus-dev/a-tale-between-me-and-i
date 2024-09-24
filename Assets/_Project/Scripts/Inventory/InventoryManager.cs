using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Item;
using ATBMI.Gameplay.Event;

namespace ATBMI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<ItemController> items;
        public List<ItemController> Item => items;

        public ItemList itemList;
        private readonly List<InventoryItem> inventoryList = new();
        private readonly Dictionary<int, ItemData> itemDetailsDictionary = new();

        public static InventoryManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Keep this instance alive across scenes
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate instance
            }

            PopulateItemDetailsDictionary();
            CheckForStartingItems();
        }

        private void PopulateItemDetailsDictionary()
        {
            foreach (ItemData item in itemList.itemList)
            {
                itemDetailsDictionary.Add(item.ItemId, item);
            }
        }

        private void CheckForStartingItems()
        {
            foreach (ItemData item in itemList.itemList)
            {
                if (item.IsStartingItem)
                {
                    AddItemToInventory(item.ItemId);
                }
            }
        }

        public ItemData GetItemDetails(int itemId)
        {
            if (itemDetailsDictionary.TryGetValue(itemId, out ItemData itemDetails))
            {
                return itemDetails;
            }

            return null;
        }

        public void AddItemToInventory(Item item, int itemId)
        {
            if (AddItemToInventory(itemId))
            {
                // Destroy the item in the scene after pickup
                Destroy(item.gameObject);
            }
        }

        public bool AddItemToInventory(int itemId)
        {
            if (itemDetailsDictionary.TryGetValue(itemId, out ItemData itemDetails))
            {
                inventoryList.Add(new InventoryItem(itemId));
                PlayerEvents.UpdateInventoryEvent(inventoryList);
                Debug.Log("Item with the ID of: " + itemId + " added to the inventory list");
            }
            else
            {
                Debug.LogWarning("There is no such item with the item ID of: " + itemId + " in the ITEM LIST. CHECK!!");
            }

            return itemDetails;
        }

        public void RemoveItemFromInventory(int itemId)
        {
            // seacrh the item inside the inventory list. If match with item id return default, if not return null
            InventoryItem itemToRemove = inventoryList.FirstOrDefault(x => x.itemId == itemId);
            if (itemToRemove != null)
            {
                inventoryList.Remove(itemToRemove);

                PlayerEvents.UpdateInventoryEvent(inventoryList);

                Debug.Log("Item with the ID of: " + itemId + " removed from the inventory list");
            }
            else
            {
                Debug.LogWarning("There is no such item with the item ID of: " + itemId + " in the ITEM LIST. CHECK!!");
            }
        }

        public int GetStartingItemCount()
        {
            return itemList.itemList.Count(x => x.IsStartingItem);
        }
    }
}
