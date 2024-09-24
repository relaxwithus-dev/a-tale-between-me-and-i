using UnityEngine;

namespace ATBMI.Inventory
{
    public class UiPlayerInventorySlotFactory : IInventorySlotFactory<UiPlayerInventorySlot>
    {
        private GameObject playerInventorySlotPrefab;
        private Transform parentTransform;

        public UiPlayerInventorySlotFactory(GameObject playerInventorySlotPrefab, Transform parentTransform)
        {
            this.playerInventorySlotPrefab = playerInventorySlotPrefab;
            this.parentTransform = parentTransform;
        }

        public UiPlayerInventorySlot CreateInventorySlot()
        {
            GameObject newSlot = Object.Instantiate(playerInventorySlotPrefab, parentTransform);
            return newSlot.GetComponent<UiPlayerInventorySlot>();
        }
    }
}
