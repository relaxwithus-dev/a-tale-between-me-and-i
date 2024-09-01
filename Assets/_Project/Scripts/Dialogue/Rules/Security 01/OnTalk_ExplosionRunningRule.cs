using ATMBI.Gameplay.Event;
using UnityEngine;

namespace ATBMI
{
    public class OnTalk_ExplosionRunningRule : IDialogueRule<RE_Security_01>
    {
        public int RulePriority => (int)RulePrioritySecurity_01.OnTalk_ExplosionRunningRule;

        public bool Evaluate(RE_Security_01 context)
        {
            Debug.Log(context.IsAfterExplosion && context.IsRunning);
            return context.IsAfterExplosion && context.IsRunning;
        }

        public void Execute(RE_Security_01 context)
        {
            context.VisitedCount++;

            if (context.VisitedCount == 1 && !context.isOnce02)
            {
                context.IsDialogueAboutToStart = true;
                PlayerEventHandler.MoveToPlayerEvent(context.onTalk_AfterExplosion_WithRunning_Visited_01, context.playerEntryPoint.position.x, context.npc.isFacingRight);
                if (context.isOnce_AfterExplosion_WithRunning_Visited_01)
                {
                    context.isOnce02 = true;
                }
            }
            else if (context.VisitedCount == 2 && !context.isOnce03)
            {
                context.IsDialogueAboutToStart = true;
                PlayerEventHandler.MoveToPlayerEvent(context.onTalk_AfterExplosion_WithRunning_Visited_02, context.playerEntryPoint.position.x, context.npc.isFacingRight);
                if (context.isOnce_AfterExplosion_WithRunning_Visited_02)
                {
                    context.isOnce03 = true;
                }
            }
            else if (context.VisitedCount == 3 && !context.isOnce04)
            {
                context.IsDialogueAboutToStart = true;
                PlayerEventHandler.MoveToPlayerEvent(context.onTalk_AfterExplosion_WithRunning_Visited_03, context.playerEntryPoint.position.x, context.npc.isFacingRight);
                if (context.isOnce_AfterExplosion_WithRunning_Visited_03)
                {
                    context.isOnce04 = true;
                }
            }
        }
    }

}
