using Ink.Runtime;
using ATBMI.Minigame;
using ATBMI.Inventory;
using ATBMI.Interaction;
using ATBMI.Gameplay.Event;
using ATBMI.Scene.Chapter;

namespace ATBMI.Dialogue
{
    public class InkExternalFunctions
    {
        // Interact status
        private readonly string TakeItem = "Take";
        private readonly string GiveItem = "Give";
        
        public void Bind(Story story)
        {
            story.BindExternalFunction("AddItem", (string itemId) => AddItem(itemId));
            story.BindExternalFunction("RemoveItem", (string itemId) => RemoveItem(itemId));
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
            story.BindExternalFunction("EnterMinigame", EnterMinigame);
            story.BindExternalFunction("UpdateStoryChapter", (string chapter) => UpdateStoryChapter(chapter));
        }
        
        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("AddItem");
            story.UnbindExternalFunction("RemoveItem");
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("FinishQuest");
            story.UnbindExternalFunction("EnterMinigame");
            story.UnbindExternalFunction("UpdateStoryChapter");
        }
        
        public void AddItem(string itemId) => UpdateItem(itemId, isAdding: true);
        public void RemoveItem(string itemId) => UpdateItem(itemId, isAdding: false);

        private void UpdateItem(string itemId, bool isAdding)
        {
            if (!int.TryParse(itemId, out var id)) return;

            if (isAdding)
                InventoryManager.Instance.AddItemToInventory(id);
            else
                InventoryManager.Instance.RemoveItemFromInventory(id);

            if (InteractObserver.GetInteractable() is CharacterInteract target)
                target.ChangeStatus(isAdding ? TakeItem : GiveItem);
        }
        
        public void StartQuest(string questId) => UpdateQuest(questId, isStarted: true);
        public void FinishQuest(string questId) => UpdateQuest(questId, isStarted: false);

        private void UpdateQuest(string questId, bool isStarted)
        {
            if (!int.TryParse(questId, out var id)) return;
            
            if (isStarted)
                QuestEvents.StartQuestEvent(id);
            else
                QuestEvents.FinishQuestEvent(id);
        }

        public void EnterMinigame() => MinigameManager.EnterMinigameEvent();
        public void UpdateStoryChapter(string chapter) => ChapterViewer.Instance.UpdateChapter(chapter);

    }
}
