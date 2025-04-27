using System.Collections;
using System.Collections.Generic;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI
{
     [CreateAssetMenu(fileName = "OnTalk_Walk", menuName = "Data/Dialogue Rules/Ayah Dewa/OnTalk_Walk")]
    public class OnTalk_Walk : DialogueRuleBase<RE_AyahDewa>
    {
        public override bool Evaluate(RE_AyahDewa context)
        {
            return !context.IsRunning && !context.isOnce03;
        }

        public override void Execute(RE_AyahDewa context)
        {
            context.IsDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(context, context.JalanBali_WalkWithInteraction, context.playerEntryPoint.position.x, context.transform.position.x, context.npc.IsFacingRight);
                if (context.isOnce_JalanBali_WalkWithInteraction)
                {
                    context.isOnce03 = true;
                }
        }
    }
}
