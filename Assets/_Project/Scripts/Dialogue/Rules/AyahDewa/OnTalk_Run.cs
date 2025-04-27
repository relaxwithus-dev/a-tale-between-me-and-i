using System.Collections;
using System.Collections.Generic;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI
{
     [CreateAssetMenu(fileName = "OnTalk_Run", menuName = "Data/Dialogue Rules/Ayah Dewa/OnTalk_Run")]
    public class OnTalk_Run : DialogueRuleBase<RE_AyahDewa>
    {
        public override bool Evaluate(RE_AyahDewa context)
        {
            return context.IsRunning && !context.isOnce02;
        }

        public override void Execute(RE_AyahDewa context)
        {
            context.IsDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(context, context.JalanBali_RunWithInteraction, context.playerEntryPoint.position.x, context.transform.position.x, context.npc.IsFacingRight);
                if (context.isOnce_JalanBali_RunWithInteraction)
                {
                    context.isOnce02 = true;
                }
        }
    }
}
