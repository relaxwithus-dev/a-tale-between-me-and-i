using ATBMI.Gameplay.Handler;
using UnityEngine;

namespace ATBMI.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemId; // This ID should correspond to the inventory item ID

        private bool isPlayerInRange;

        private void Awake()
        {
            // TODO: Change the method

            isPlayerInRange = false;
        }

        private void Update()
        {
            // Check if player presses interact button to pick up the item
            if (isPlayerInRange && GameInputHandler.Instance.IsTapInteract)
            {
                PickupItem();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Enable pickup action when the player collides with the item
                isPlayerInRange = true;

                Debug.Log("Press interact button to pick up: " + itemId);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Player has left the item's range
                isPlayerInRange = false;

                Debug.Log("Left item range.");
            }
        }

        private void PickupItem()
        {
            InventoryManager.Instance.AddItemToInventory(this, itemId);
        }
    }
}
