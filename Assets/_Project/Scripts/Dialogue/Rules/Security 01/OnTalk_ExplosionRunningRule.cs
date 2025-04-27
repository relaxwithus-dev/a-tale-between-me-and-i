using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Dialogue
{
    [CreateAssetMenu(fileName = "OnTalk_ExplosionRunningRule", menuName = "Data/Dialogue Rules/Security_01/OnTalk_ExplosionRunningRule")]
    public class OnTalk_ExplosionRunningRule : DialogueRuleBase<RE_Security_01>
    {
        // public int RulePriority => (int)RulePrioritySecurity_01.OnTalk_ExplosionRunningRule;

        public override bool Evaluate(RE_Security_01 context)
        {
            Debug.Log("OnTalk_ExplosionRunningRule" + (context.IsAfterExplosion && context.IsRunning));
            return context.IsAfterExplosion && context.IsRunning;
        }

        public override void Execute(RE_Security_01 context)
        {
            context.VisitedCount++;

            if (context.VisitedCount == 1 && !context.isOnce02)
            {
                context.IsDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(context, context.onTalkedTo_AfterExplosion_WithRunning_Visited_01, context.playerEntryPoint.position.x, context.transform.position.x, context.npc.IsFacingRight);
                if (context.isOnce_AfterExplosion_WithRunning_Visited_01)
                {
                    context.isOnce02 = true;
                }
            }
            else if (context.VisitedCount == 2 && !context.isOnce03)
            {
                context.IsDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(context, context.onTalkedTo_AfterExplosion_WithRunning_Visited_02, context.playerEntryPoint.position.x, context.transform.position.x, context.npc.IsFacingRight);
                if (context.isOnce_AfterExplosion_WithRunning_Visited_02)
                {
                    context.isOnce03 = true;
                }
            }
            else if (context.VisitedCount == 3 && !context.isOnce04)
            {
                context.IsDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(context, context.onTalkedTo_AfterExplosion_WithRunning_Visited_03, context.playerEntryPoint.position.x, context.transform.position.x, context.npc.IsFacingRight);
                if (context.isOnce_AfterExplosion_WithRunning_Visited_03)
                {
                    context.isOnce04 = true;
                }
            }
        }
    }

}
