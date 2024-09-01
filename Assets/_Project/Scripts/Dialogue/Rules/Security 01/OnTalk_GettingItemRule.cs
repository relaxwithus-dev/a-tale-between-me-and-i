using UnityEngine;

namespace ATBMI
{
    public class OnTalk_GettingItemRule : IDialogueRule<RE_Security_01>
    {
        public int RulePriority => (int)RulePrioritySecurity_01.OnTalk_GettingItemRule;

        public bool Evaluate(RE_Security_01 context)
        {
            return context.IsAfterGettingItem;
        }

        public void Execute(RE_Security_01 context)
        {
            // if (InventoryManager.Instance.LastItem.itemName == "Key")
            // {
            //     DialogueManager.Instance.EnterDialogueMode(context.onTalk_AfterGettingAKey);
            // context.IsDialogueAboutToStart = true;
            // }
            // else if (InventoryManager.Instance.LastItem.itemName == "Rock")
            // {
            //     DialogueManager.Instance.EnterDialogueMode(context.onTalk_AfterGettingARock);
            // context.IsDialogueAboutToStart = true;
            // }

            Debug.Log("Get an item");
        }
    }
}
