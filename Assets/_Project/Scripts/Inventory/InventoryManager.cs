using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Interaction;

namespace ATBMI.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> collectibleItem;
        public List<GameObject> CollectibleItem => collectibleItem;
    }
}
