using ATBMI.Data;
using ATBMI.Inventory;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class ItemInteraction : Interaction
    {
        #region Fields & Properties
        
        [Header("Item")]
        [SerializeField] private ItemData itemData;
        [SerializeField] private TextAsset dialogueAsset;

        #endregion

        #region Methods

        // TODO: Drop method call dialogue
        public override void Interact(InteractManager manager, int itemId = 0)
        {
            base.Interact(manager, itemId);
            Debug.Log($"take item {itemData.ItemName}");
            InventoryManager.Instance.AddItemToInventory(itemId, this);
        }

        #endregion
    }
}