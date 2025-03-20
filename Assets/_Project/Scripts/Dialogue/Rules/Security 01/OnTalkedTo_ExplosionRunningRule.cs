using UnityEngine;

namespace ATBMI.Dialogue
{
    [CreateAssetMenu(fileName = "OnTalkedTo_ExplosionRunningRule", menuName = "Data/Dialogue Rules/Security_01/OnTalkedTo_ExplosionRunningRule")]
    public class OnTalkedTo_ExplosionRunningRule : DialogueRuleBase<RE_Security_01>
    {
        // public int RulePriority => (int)RulePrioritySecurity_01.OnTalkedTo_ExplosionRunningRule;

        public override bool Evaluate(RE_Security_01 context)
        {
            return context.isPlayerInRange && context.IsAfterExplosion && context.IsRunning && !context.isOnce01;
        }

        public override void Execute(RE_Security_01 context)
        {
            DialogueManager.Instance.EnterDialogueMode(context.onTalkedTo_AfterExplosion_WithRunning);
            if (context.isOnce_AfterExplosion_WithRunning)
            {
                context.isOnce01 = true;
            }
        }
    }
}
