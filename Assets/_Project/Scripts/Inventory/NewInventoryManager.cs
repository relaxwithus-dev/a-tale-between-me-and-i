using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ATMBI.Gameplay.Event;

namespace ATBMI.Inventory
{
    public class NewInventoryManager : MonoBehaviour
    {
        public SO_ItemList itemList;

        private List<InventoryItem> inventoryList = new List<InventoryItem>();
        private Dictionary<int, SO_ItemDetails> itemDetailsDictionary = new Dictionary<int, SO_ItemDetails>();

        public static NewInventoryManager Instance;
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

        private void Update()
        {
            // TEST
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RemoveItemFromInventory(1);
            }
        }

        private void PopulateItemDetailsDictionary()
        {
            foreach (SO_ItemDetails item in itemList.itemList)
            {
                itemDetailsDictionary.Add(item.itemId, item);
            }
        }

        private void CheckForStartingItems()
        {
            foreach (SO_ItemDetails item in itemList.itemList)
            {
                if (item.isStartingItem)
                {
                    AddItemToInventory(item.itemId);
                }
            }
        }

        public SO_ItemDetails GetItemDetails(int itemId)
        {
            if (itemDetailsDictionary.TryGetValue(itemId, out SO_ItemDetails itemDetails))
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
            if (itemDetailsDictionary.TryGetValue(itemId, out SO_ItemDetails itemDetails))
            {
                inventoryList.Add(new InventoryItem(itemId));

                PlayerEventHandler.UpdateInventoryEvent(inventoryList);

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

                PlayerEventHandler.UpdateInventoryEvent(inventoryList);

                Debug.Log("Item with the ID of: " + itemId + " removed from the inventory list");
            }
            else
            {
                Debug.LogWarning("There is no such item with the item ID of: " + itemId + " in the ITEM LIST. CHECK!!");
            }
        }

        public int GetStartingItemCount()
        {
            return itemList.itemList.Count(x => x.isStartingItem);
        }
    }
}
