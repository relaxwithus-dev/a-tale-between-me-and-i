using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Gameplay.Event;
using ATBMI.Interaction;
using System.Collections;

namespace ATBMI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        #region Fields & Properties

        public ItemList itemList;

        public GameObject uiGetItemPanel;

        public List<InventoryItem> InventoryList { get; set; } = new();
        public Dictionary<int, ItemData> itemDatasDict = new();

        public static InventoryManager Instance;

        #endregion

        public void Awake()
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


        }

        public void Start()
        {
            PopulateItemDict();

            // uiGetItemPanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                AddItemToInventory(101);
                AddItemToInventory(102);
                AddItemToInventory(103);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                AddItemToInventory(104);
                AddItemToInventory(105);
                AddItemToInventory(106);
            }
        }

        public void PopulateItemDict()
        {
            foreach (ItemData item in itemList.itemList)
            {
                if (itemDatasDict.ContainsKey(item.ItemId))
                {
                    // Debug.LogWarning("Duplicate ID found when creating item data dictionary: " + item.ItemId);
                }
                else
                {
                    itemDatasDict.Add(item.ItemId, item);
                }

            }
        }

        private IEnumerator AnimateUIGetItemPanel()
        {
            uiGetItemPanel.SetActive(true);

            yield return new WaitForSeconds(2f);

            uiGetItemPanel.SetActive(false);
        }

        public ItemData GetItemData(int itemId)
        {
            if (itemDatasDict.TryGetValue(itemId, out ItemData itemData))
            {
                return itemData;
            }
            return null;
        }

        public void AddItemToInventory(int itemId, ItemInteract item = null)
        {
            if (itemDatasDict.TryGetValue(itemId, out ItemData data))
            {
                // Add item to inventory
                InventoryList.Add(new InventoryItem(itemId));
                PlayerEvents.UpdateInventoryEvent(InventoryList);
                // Debug.Log("add item " + data.ItemName + " " + data.ItemId + " to inventory");

                // TODO: change this method to UI manager
                // StartCoroutine(AnimateUIGetItemPanel());

                // Destroy item
                if (item != null)
                    Destroy(item.gameObject);
            }
            else
            {
                Debug.LogError("failed to acquire item with id " + itemId);
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
                Debug.Log("remove item with id " + itemId + " from inventory");
            }
            else
            {
                Debug.LogError("failed to remove item with id " + itemId);
            }
        }
    }
}
