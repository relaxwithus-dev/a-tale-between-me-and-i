using ATBMI.Data;
using ATBMI.Inventory;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class ItemInteraction : Interaction
    {
        [SerializeField] private ItemData itemData;
        
        public override void Interact(InteractManager manager, int itemId = 0)
        {
            base.Interact(manager, itemId);
            StopInteract();
            Debug.Log($"take item {itemData.ItemName}");
            InventoryManager.Instance.AddItemToInventory(itemId, this);
        }
    }
}