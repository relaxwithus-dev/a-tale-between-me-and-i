using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Gameplay.Event;
using ATBMI.Interaction;
using System.Collections;
using TMPro;
using DG.Tweening;

namespace ATBMI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField] private ItemList itemList;

        [SerializeField] private GameObject uiGetItemPanel;
        [SerializeField] private CanvasGroup uiItemCanvasGroup;
        [SerializeField] private TextMeshProUGUI uiItemInfo;

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

            uiGetItemPanel.SetActive(false);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                AddItemToInventory(1);
                // AddItemToInventory(2);
                // AddItemToInventory(3);
            }

            if(Input.GetKeyDown(KeyCode.V))
            {
                AddItemToInventory(4);
                // AddItemToInventory(5);
                // AddItemToInventory(6);
            }
        }

        private void PopulateItemDict()
        {
            foreach (ItemData item in itemList.itemList)
            {
                if (itemDatasDict.ContainsKey(item.ItemId))
                {
                    Debug.LogWarning("Duplicate ID found when creating item data dictionary: " + item.ItemId);
                }
                else
                {
                    itemDatasDict.Add(item.ItemId, item);
                }

            }
        }

        private IEnumerator AnimateUIGetItemPanel(ItemData itemData)
        {
            uiGetItemPanel.SetActive(true);

            uiItemCanvasGroup.alpha = 0f;

            uiItemInfo.text = itemData.ItemName;

            // Fade in
            uiItemCanvasGroup.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(2.5f);

            // Fade out
            uiItemCanvasGroup.DOFade(0f, 0.5f);
            yield return new WaitForSeconds(0.5f);

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
                QuestEvents.GetItemQuestStepEvent(itemId);
                Debug.Log("add item " + data.ItemName + " " + data.ItemId + " to inventory");

                // TODO: change this method to UI manager
                StartCoroutine(AnimateUIGetItemPanel(data));

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
