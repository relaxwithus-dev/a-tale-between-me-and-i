using UnityEngine;
using UnityEngine.UI;

namespace ATBMI.Inventory
{
    public class UiInventorySlot : MonoBehaviour
    {
        public Image image;
        public string itemName;
        public string itemDescription;

        public SO_ItemDetails itemDetails;

        private void Start()
        {
            // Initialize slot
        }
    }
}
