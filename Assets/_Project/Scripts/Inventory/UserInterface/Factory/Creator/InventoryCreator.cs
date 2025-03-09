using System;
using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Inventory
{
    public class InventoryCreator : InventoryBarCreator
    {
        #region Methods

        protected override void InitOnStart()
        {
            base.InitOnStart();
            inventorySlot = new InventorySlot(inventoryPrefab, inventoryParent);
        }

        protected override void ModifyInventory(List<InventoryItem> inventoryList)
        {
            base.ModifyInventory(inventoryList);

            int itemSlotCount = inventoryFlags.Count;
            int inventoryCount = inventoryList.Count;

            if (inventoryCount > itemSlotCount)
            {
                for (int i = itemSlotCount; i < inventoryCount; i++)
                {
                    CreateInventorySlot();
                }
            }
            else if (inventoryCount < itemSlotCount)
            {
                for (int i = itemSlotCount - 1; i >= inventoryCount; i--)
                {
                    RemoveInventorySlot(i);
                }
            }
        }
        
        #endregion

        // protected override void ModifyInventory(List<InventoryItem> inventoryList)
        // {
        //     // Clear existing inventory slots
        //         foreach (var slot in inventoryFlags)
        //         {
        //             if (slot is UiPlayerInventorySlot playerSlot)
        //             {
        //                 playerSlot.itemDetails = null;
        //                 playerSlot.image.sprite = null;
        //                 playerSlot.itemName = "";
        //             }
        //             else if (slot is UiInventorySlot uiSlot)
        //             {
        //                 uiSlot.itemDetails = null;
        //                 uiSlot.image.sprite = null;
        //                 uiSlot.itemName = "";
        //                 uiSlot.itemDescription = "";
        //             }
        //         }

        //     // Update inventory slots with new items
        //     for (int i = 0; i < inventoryFlags.Count; i++)
        //     {
        //         if (i < inventoryList.Count)
        //         {
        //             ItemData itemDetails = InventoryManager.Instance.GetItemData(inventoryList[i].ItemId);
        //             if (itemDetails != null)
        //             {
        //                 if (inventoryFlags[i] is UiPlayerInventorySlot playerSlot)
        //                 {
        //                     playerSlot.itemDetails = itemDetails;
        //                     playerSlot.image.sprite = itemDetails.ItemSprite;
        //                     playerSlot.itemName = itemDetails.ItemName;
        //                 }
        //                 else if (inventoryFlags[i] is UiInventorySlot uiSlot)
        //                 {
        //                     uiSlot.itemDetails = itemDetails;
        //                     uiSlot.image.sprite = itemDetails.ItemSprite;
        //                     uiSlot.itemName = itemDetails.ItemName;
        //                     uiSlot.itemDescription = itemDetails.ItemDescription;
        //                 }
        //             }
        //         }
        //     }

        //     // Create or destroy inventory slots as needed
        //     if (inventoryList.Count > inventoryFlags.Count)
        //     {
        //         for (int i = inventoryFlags.Count; i < inventoryList.Count; i++)
        //         {
        //             CreateInventorySlot();
        //         }
        //     }
        //     else if (inventoryList.Count < inventoryFlags.Count)
        //     {
        //         for (int i = inventoryFlags.Count - 1; i >= inventoryList.Count; i--)
        //         {
        //             DestroyInventorySlot(i);
        //         }
        //     }
        // }
    }
}