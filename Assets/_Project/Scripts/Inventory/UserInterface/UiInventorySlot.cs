using UnityEngine;
using UnityEngine.UI;
using ATBMI.Data;

namespace ATBMI.Inventory
{
    public class UiInventorySlot : MonoBehaviour
    {
        public Image image;
        public string itemName;
        public string itemDescription;

        public ItemData itemDetails;

        private void Start()
        {
            // Initialize slot
        }
    }
}
