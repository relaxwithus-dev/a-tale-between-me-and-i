using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ATBMI.Enum;
using ATBMI.Inventory;

namespace ATBMI.Interaction
{
    public class InteractDataFactory : IInteractDataFactory
    {
        private readonly InventoryManager _inventoryManager;

        public InteractDataFactory(InventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }

        public InteractData CreateInteractData(int index, Button button)
        {
            var key = GetInteractKey(index);
            var type = GetInteractType(index);

            InteractData data = new()
            {
                Id = key,
                Type = type,
                Button = button
            };

            switch (data.Id)
            {
                case 0:
                    data.Description = "Berbicara";
                    break;
                case 1:
                    data.Description = "Keluar";
                    break;
                default:
                    var item = _inventoryManager.CollectibleItem[index - 2];
                    if (item.TryGetComponent<CollectibleInteract>(out var collectible))
                    {
                        data.Interactable = collectible;
                        data.Description = collectible.CollectibleName;
                    }
                    break;
            }
            
            return data;
        }

        // !- Helpers
        private int GetInteractKey(int index)
        {
            if (index < 2)
            {
                return index;
            }
            else
            {
                var inventoryItem = _inventoryManager.CollectibleItem[index - 2].GetComponent<CollectibleInteract>();
                return inventoryItem.InteractId;
            }
        }

        private InteractType GetInteractType(int index)
        {
            return index switch
            {
                0 => InteractType.Talks,
                1 => InteractType.Close,
                _ => InteractType.Item
            };
        }
    }
}