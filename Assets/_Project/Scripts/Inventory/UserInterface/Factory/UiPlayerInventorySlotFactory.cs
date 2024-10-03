using UnityEngine;

namespace ATBMI.Inventory
{
    public class UiPlayerInventorySlotFactory : IInventorySlotFactory<UiPlayerInventorySlot>
    {
        private readonly GameObject inventorySlotPrefab;
        private readonly Transform parentTransform;

        public UiPlayerInventorySlotFactory(GameObject inventorySlotPrefab, Transform parentTransform)
        {
            this.inventorySlotPrefab = inventorySlotPrefab;
            this.parentTransform = parentTransform;
        }

        public UiPlayerInventorySlot CreateInventorySlot()
        {
            GameObject newSlot = Object.Instantiate(inventorySlotPrefab, parentTransform);
            return newSlot.GetComponent<UiPlayerInventorySlot>();
        }
    }
}
