using UnityEngine;
using ATBMI.Data;
using ATBMI.Inventory;

namespace ATBMI.Interaction
{
    public class ItemInteract : MonoBehaviour, IInteractable
    {
        [Header("Attribute")]
        [SerializeField] private ItemData itemData;
        [SerializeField] private Transform signTransform;
        
        // Core
        public Transform GetSignTransform() => signTransform;
        public void Interact(InteractManager manager, int itemId = 0)
        {
            Debug.Log($"take item {itemData.ItemName}");
            InventoryManager.Instance.AddItemToInventory(itemId, this);
        }
    }
}