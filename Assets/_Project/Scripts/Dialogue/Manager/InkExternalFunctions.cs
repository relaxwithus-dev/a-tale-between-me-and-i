using UnityEngine;
using Ink.Runtime;
using ATBMI.Gameplay.Event;
using Unity.VisualScripting;
using ATBMI.Inventory;

namespace ATBMI.Dialogue
{
    public class InkExternalFunctions
    {
        public void Bind(Story story, Animator emoteAnimator)
        {
            story.BindExternalFunction("PlayEmote", (string emoteName) => PlayEmote(emoteName, emoteAnimator));
            story.BindExternalFunction("QuestInteract", (string questState) => QuestInteract(questState));
            story.BindExternalFunction("AddItem", (string itemId) => AddItem(itemId));
            story.BindExternalFunction("RemoveItem", (string itemId) => RemoveItem(itemId));
        }

        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("PlayEmote");
            story.UnbindExternalFunction("QuestInteract");
            story.UnbindExternalFunction("AddItem");
            story.UnbindExternalFunction("RemoveItem");
        }

        public void PlayEmote(string emoteName, Animator emoteAnimator)
        {
            if (emoteAnimator != null)
            {
                emoteAnimator.Play(emoteName);
            }
            else
            {
                Debug.LogWarning("Tried to play emote, but emote animator was "
                    + "not initialized when entering dialogue mode.");
            }
        }

        public void QuestInteract(string questState)
        {
            QuestEvents.QuestInteractEvent(questState);
        }

        public void AddItem(string itemId)
        {
            int id;
            if (int.TryParse(itemId, out id))
            {
                InventoryManager.Instance.AddItemToInventory(id);
            }
            else
            {
                Debug.LogError("Invalid item ID: " + itemId);
            }
        }

        public void RemoveItem(string itemId)
        {
            int id;
            if (int.TryParse(itemId, out id))
            {
                InventoryManager.Instance.RemoveItemFromInventory(id);
            }
            else
            {
                Debug.LogError("Invalid item ID: " + itemId);
            }
        }
    }
}
