using UnityEngine;

namespace ATBMI.Inventory
{
    public class InventorySlot : IInventorySlot
    {
        private readonly GameObject inventoryPrefab;
        private readonly Transform parentTransform;

        public InventorySlot(GameObject prefab, Transform parent)
        {
            this.inventoryPrefab = prefab;
            this.parentTransform = parent;
        }

        public FlagBase CreateInventorySlot()
        {
            GameObject slotObj = Object.Instantiate(inventoryPrefab,parentTransform, worldPositionStays: false);
            return slotObj.GetComponent<InventoryFlag>();
        }

    }
}
