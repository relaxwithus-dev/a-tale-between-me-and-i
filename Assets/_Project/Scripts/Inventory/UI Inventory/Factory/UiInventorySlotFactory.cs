using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Inventory
{
    public class UiInventorySlotFactory : IInventorySlotFactory<UiInventorySlot>
    {
        private GameObject uiInventorySlotPrefab;
        private Transform parentTransform;

        public UiInventorySlotFactory(GameObject uiInventorySlotPrefab, Transform parentTransform)
        {
            this.uiInventorySlotPrefab = uiInventorySlotPrefab;
            this.parentTransform = parentTransform;
        }

        public UiInventorySlot CreateInventorySlot()
        {
            GameObject newSlot = Object.Instantiate(uiInventorySlotPrefab, parentTransform);
            return newSlot.GetComponent<UiInventorySlot>();
        }
    }
}
