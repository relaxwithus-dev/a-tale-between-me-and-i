using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Interaction;
using ATBMI.Item;

namespace ATBMI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<ItemController> item;
        public List<ItemController> Item => item;
    }
}
