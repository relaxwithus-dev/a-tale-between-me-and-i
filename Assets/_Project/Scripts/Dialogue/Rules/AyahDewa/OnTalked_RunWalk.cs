using System.Collections;
using System.Collections.Generic;
using ATBMI.Dialogue;
using UnityEngine;

namespace ATBMI
{
     [CreateAssetMenu(fileName = "OnTalked_RunWalk", menuName = "Data/Dialogue Rules/Ayah Dewa/OnTalked_RunWalk")]
    public class OnTalked_RunWalk : DialogueRuleBase<RE_AyahDewa>
    {
        public override bool Evaluate(RE_AyahDewa context)
        {
            return context.IsPlayerInRange && context.IsRunning && !context.isOnce01;
        }

        public override void Execute(RE_AyahDewa context)
        {
            DialogueManager.Instance.EnterDialogueMode(context.onTalkedTo_JalanBali_RunOrWalkWOInteract);
            if (context.isOnce_JalanBali_RunOrWalkWOInteract)
            {
                context.isOnce01 = true;
            }
        }
    }
}
