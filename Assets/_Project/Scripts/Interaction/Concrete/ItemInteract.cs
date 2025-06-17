using System;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Inventory;

namespace ATBMI.Interaction
{
    public class ItemInteract : MonoBehaviour, IInteractable
    {
        [Header("Attribute")]
        [SerializeField] private bool isCollected;
        [SerializeField] private ItemData itemData;
        [SerializeField] private Transform signTransform;
        
        // Unity Callbacks
        private void Start()
        {
            // Setup item
            var itemId = itemData.ItemId;
            var inventoryItem = InventoryManager.Instance.GetInventoryItemById(itemId);
            
            isCollected = inventoryItem != null;
            gameObject.SetActive(!isCollected);
        }
        
        // Core
        public Transform GetSignTransform() => signTransform;
        public void Interact(int itemId = 0)
        {
            var itemIdToCollect = itemId == 0 ? itemData.ItemId : itemId;
            InventoryManager.Instance.AddItemToInventory(itemIdToCollect, this);
        }
    }
}