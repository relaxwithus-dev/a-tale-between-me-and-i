using Ink.Runtime;
using ATBMI.Inventory;
using ATBMI.Interaction;
using ATBMI.Gameplay.Event;

namespace ATBMI.Dialogue
{
    public class InkExternalFunctions
    {
        private readonly string TakeItem = "Take";
        private readonly string GiveItem = "Give";
        
        public void Bind(Story story)
        {
            story.BindExternalFunction("AddItem", (string itemId) => AddItem(itemId));
            story.BindExternalFunction("RemoveItem", (string itemId) => RemoveItem(itemId));
            story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
            story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
        }
        
        public void Unbind(Story story)
        {
            story.UnbindExternalFunction("AddItem");
            story.UnbindExternalFunction("RemoveItem");
            story.UnbindExternalFunction("StartQuest");
            story.UnbindExternalFunction("FinishQuest");
        }
        
        public void AddItem(string itemId) => UpdateItem(itemId, true);
        public void RemoveItem(string itemId) => UpdateItem(itemId, false);

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
