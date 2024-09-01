namespace ATBMI
{
    public class OnTalkedTo_ExplosionRunningRule : IDialogueRule<RE_Security_01>
    {
        public int RulePriority => (int)RulePrioritySecurity_01.OnTalkedTo_ExplosionRunningRule;

        public bool Evaluate(RE_Security_01 context)
        {
            return context.IsPlayerInRange && context.IsAfterExplosion && context.IsRunning && !context.isOnce01;
        }

        public void Execute(RE_Security_01 context)
        {
            DialogueManager.Instance.EnterDialogueMode(context.onTalkedTo_AfterExplosion_WithRunning);
            if (context.isOnce_AfterExplosion_WithRunning)
            {
                context.isOnce01 = true;
            }
        }
    }
}
