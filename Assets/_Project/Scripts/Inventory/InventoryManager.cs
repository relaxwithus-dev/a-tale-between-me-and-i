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
        private readonly Dictionary<int, ItemData> itemDetailsDict = new();

        public static InventoryManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            PopulateItemDict();
        }

        private void PopulateItemDict()
        {
            foreach (ItemData item in itemList.itemList)
            {
                itemDetailsDict.Add(item.ItemId, item);
            }
        }

        public ItemData GetItemData(int itemId)
        {
            if (itemDetailsDict.TryGetValue(itemId, out ItemData itemDetails))
            {
                return itemDetails;
            }
            return null;
        }

        public void AddItemToInventory(Item item, int itemId)
        {
            if (itemDetailsDict.TryGetValue(itemId, out ItemData data))
            {
                // Add item to inventory
                inventoryList.Add(new InventoryItem(itemId));
                PlayerEvents.UpdateInventoryEvent(inventoryList);
                Debug.Log($"add item {data.ItemName} ({data.ItemId}) to inventory");

                // Destroy item
                Destroy(item.gameObject);
            }
            else
            {
                Debug.LogError($"failed to acquire item with id {itemId}");
            }
        }

        public void RemoveItemFromInventory(int itemId)
        {
            // Search avail item id before remove
            InventoryItem itemToRemove = inventoryList.FirstOrDefault(x => x.ItemId == itemId);

            if (itemToRemove != null)
            {
                inventoryList.Remove(itemToRemove);
                PlayerEvents.UpdateInventoryEvent(inventoryList);
                Debug.Log($"remove item with id {itemId} from inventory");
            }
            else
            {
                Debug.LogError($"failed to remove item with id {itemId}");
            }
        }

        public int GetStartingItemCount()
        {
            return itemList.itemList.Count(x => x.IsStartingItem);
        }
    }
}
