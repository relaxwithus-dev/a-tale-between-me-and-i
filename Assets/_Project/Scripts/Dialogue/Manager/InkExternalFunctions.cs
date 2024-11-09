using UnityEngine;
using Ink.Runtime;
using ATBMI.Gameplay.Event;
using ATBMI.Inventory;
using ATBMI.Enum;

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
            // Try to parse the string to the enum
            if (System.Enum.TryParse(questState, out QuestStateEnum questStateChecker))
            {
                QuestStateEnum questStateEnum = QuestStateEnum.Can_Start;

                // Use switch statement on the enum
                switch (questStateChecker)
                {
                    case QuestStateEnum.Can_Start:
                        questStateEnum = QuestStateEnum.Can_Start;
                        break;
                    case QuestStateEnum.Can_Finish:
                        questStateEnum = QuestStateEnum.Can_Finish;
                        break;
                    default:
                        questStateEnum = QuestStateEnum.Null;
                        Debug.Log("InvalidState " + questState);
                        break;
                }

                if (questStateEnum != QuestStateEnum.Null)
                {
                    QuestEvents.QuestInteractEvent(questStateEnum);
                }
            }
            else
            {
                Debug.Log("InvalidState " + questState);
            }

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
