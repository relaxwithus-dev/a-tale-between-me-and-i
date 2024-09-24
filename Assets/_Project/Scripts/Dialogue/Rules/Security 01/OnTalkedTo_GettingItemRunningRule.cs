namespace ATBMI.Dialogue
{
    public class OnTalkedTo_GettingItemRunningRule : IDialogueRule<RE_Security_01>
    {
        public int RulePriority => (int)RulePrioritySecurity_01.OnTalkedTo_GettingItemRunningRule;

        public bool Evaluate(RE_Security_01 context)
        {
            return context.isPlayerInRange && context.IsAfterGettingItem && context.IsRunning && !context.isOnce07;
        }

        public void Execute(RE_Security_01 context)
        {
            DialogueManager.Instance.EnterDialogueMode(context.onTalkedTo_AfterGettingAnItem, context.emoteAnimator);
            if (context.isOnce_AfterGettingItem)
            {
                context.isOnce07 = true;
            }
        }
    }
}
