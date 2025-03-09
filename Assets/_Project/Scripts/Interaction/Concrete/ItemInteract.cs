using System;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Inventory;
using ATBMI.Gameplay.Event;

namespace ATBMI.Interaction
{
    public class ItemInteract : MonoBehaviour, IInteractable
    {
        [Header("Properties")]
        [SerializeField] private ItemData itemData;
        [SerializeField] private ItemType itemType;
        [SerializeField] private Transform signTransform;
        [SerializeField] private TextAsset defaultDialogue;
        [SerializeField] private TextAsset defaultItemDialogue;
        
        private enum ItemType { Collectible, Scenery }
        
        public bool Validate() => itemType == ItemType.Scenery;
        public Transform GetSignTransform() => signTransform;
        
        public void Interact(InteractManager manager, int itemId = 0)
        {
            switch (itemType)
            {
                case ItemType.Collectible:
                    ExecuteCollectible(itemId);
                    break;
                case ItemType.Scenery:
                    ExecuteScenery(itemId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ExecuteCollectible(int itemId)
        {
            Debug.Log($"take item {itemData.ItemName}");
            InventoryManager.Instance.AddItemToInventory(itemId, this);
        }

        private void ExecuteScenery(int itemId)
        {
            if (itemId == 0)
            {
                DialogueEvents.EnterDialogueEvent(defaultDialogue);
            }
            else
            {
                // TODO: Apply dialogue, tidak terjadi apa2
                DialogueEvents.EnterDialogueEvent(defaultItemDialogue);
            }
        }
    }
}