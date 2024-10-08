using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Inventory
{
    // TODO: Pake class ini buat handle UI Inventory
    [RequireComponent(typeof(InventoryCreator))]
    public class InventoryHandler : MonoBehaviour
    {
        private InventoryCreator _inventoryCreator;

        private void Awake()
        {
            _inventoryCreator = GetComponent<InventoryCreator>();
        }
    }
}