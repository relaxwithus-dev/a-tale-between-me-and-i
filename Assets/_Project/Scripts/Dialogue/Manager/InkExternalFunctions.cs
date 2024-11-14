using UnityEngine;
using Ink.Runtime;
using ATBMI.Gameplay.Event;
using ATBMI.Inventory;
using ATBMI.Enum;
using System;

namespace ATBMI.Dialogue
{
    public class InkExternalFunctions
    {
        public void Bind(Story story, Animator emoteAnimator)
        {
            story.BindExternalFunction("PlayEmote", (string emoteName) => PlayEmote(emoteName, emoteAnimator));
            story.BindExternalFunction("AddItem", (string itemId) => AddItem(itemId));
            story.BindExternalFunction("RemoveItem", (string itemId) => RemoveItem(itemId));
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
        }

        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("PlayEmote");
            story.UnbindExternalFunction("AddItem");
            story.UnbindExternalFunction("RemoveItem");
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("FinishQuest");
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

        public void AddItem(string itemId)
        {
            InventoryManager.Instance.AddItemToInventory(int.Parse(itemId));
        }

        public void RemoveItem(string itemId)
        {
            InventoryManager.Instance.RemoveItemFromInventory(int.Parse(itemId));
        }

        public void StartQuest(string questId)
        {
            QuestEvents.StartQuestEvent(int.Parse(questId));
        }

        public void FinishQuest(string questId)
        {
            QuestEvents.FinishQuestEvent(int.Parse(questId));
        }
    }
}
