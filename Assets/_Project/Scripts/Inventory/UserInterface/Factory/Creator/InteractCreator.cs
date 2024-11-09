using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Inventory
{
    public class InteractCreator : InventoryBarCreator
    {
        #region Methods

        protected override void InitOnStart()
        {
            base.InitOnStart();
            inventorySlot = new InteractSlot(inventoryPrefab, inventoryParent);
        }

        protected override void ModifyInventory(List<InventoryItem> inventoryList)
        {
            base.ModifyInventory(inventoryList);

            var itemSlotCount = inventoryFlags.Count;
            var inventoryCount = inventoryList.Count;

            if (inventoryCount > itemSlotCount)
            {
                for (var i = itemSlotCount; i < inventoryCount; i++)
                {
                    CreateInventorySlot();
                }
            }
            else if (inventoryCount < itemSlotCount)
            {
                for (var i = itemSlotCount - 1; i >= inventoryCount; i--)
                {
                    if (itemSlotCount <= 3) continue;
                    RemoveInventorySlot(i);
                }
            }
        }

        #endregion
    }
}