using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Dialogue
{
    [CreateAssetMenu(fileName = "OnTalk_ExplosionWalkingRule", menuName = "Data/Dialogue Rules/Security_01/OnTalk_ExplosionWalkingRule")]
    public class OnTalk_ExplosionWalkingRule : DialogueRuleBase<RE_Security_01>
    {
        // public int RulePriority => (int)RulePrioritySecurity_01.OnTalk_ExplosionWalkingRule;

        public override bool Evaluate(RE_Security_01 context)
        {
            return context.IsAfterExplosion && !context.IsRunning;
        }

        public override void Execute(RE_Security_01 context)
        {
            context.VisitedCount++;

            if (context.VisitedCount == 1 && !context.isOnce05)
            {
                context.isDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(context, context.onTalk_AfterExplosion_WithWalking_Visited_01, context.playerEntryPoint.position.x, context.transform.position.x, context.npc.isFacingRight);
                if (context.isOnce_AfterExplosion_WithWalking_Visited_01)
                {
                    context.isOnce05 = true;
                }
            }
            else if (context.VisitedCount == 2 && !context.isOnce06)
            {
                context.isDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(context, context.onTalk_AfterExplosion_WithWalking_Visited_02, context.playerEntryPoint.position.x, context.transform.position.x, context.npc.isFacingRight);
                if (context.isOnce_AfterExplosion_WithWalking_Visited_02)
                {
                    context.isOnce06 = true;
                }
            }
        }
    }
}
