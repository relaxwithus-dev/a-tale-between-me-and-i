using UnityEngine;

namespace ATBMI.Inventory
{
    public class UiInventorySlotFactory : IInventorySlotFactory<UiInventorySlot>
    {
        private readonly GameObject inventorySlotPrefab;
        private readonly Transform parentTransform;

        public UiInventorySlotFactory(GameObject inventorySlot, Transform parent)
        {
            this.inventorySlotPrefab = inventorySlot;
            this.parentTransform = parent;
        }

        public UiInventorySlot CreateInventorySlot()
        {
            GameObject newSlot = Object.Instantiate(inventorySlotPrefab, parentTransform);
            return newSlot.GetComponent<UiInventorySlot>();
        }
    }
}
