using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Gameplay.Event;
using ATBMI.Interaction;

namespace ATBMI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [SerializeField] private ItemList itemList;

        public List<InventoryItem> InventoryList { get; set; } = new();
        private readonly Dictionary<int, ItemData> itemDatasDict = new();

        public static InventoryManager Instance;

        #endregion

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
                itemDatasDict.Add(item.ItemId, item);
            }
        }

        public ItemData GetItemData(int itemId)
        {
            if (itemDatasDict.TryGetValue(itemId, out ItemData itemData))
            {
                return itemData;
            }
            return null;
        }

        public void AddItemToInventory(int itemId, ItemInteraction item = null)
        {
            if (itemDatasDict.TryGetValue(itemId, out ItemData data))
            {
                // Add item to inventory
                InventoryList.Add(new InventoryItem(itemId));
                PlayerEvents.UpdateInventoryEvent(InventoryList);
                Debug.Log($"add item {data.ItemName} ({data.ItemId}) to inventory");

                // Destroy item
                if (item != null)
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
            InventoryItem itemToRemove = InventoryList.FirstOrDefault(x => x.ItemId == itemId);

            if (itemToRemove != null)
            {
                InventoryList.Remove(itemToRemove);
                PlayerEvents.UpdateInventoryEvent(InventoryList);
                Debug.Log($"remove item with id {itemId} from inventory");
            }
            else
            {
                Debug.LogError($"failed to remove item with id {itemId}");
            }
        }
    }
}
