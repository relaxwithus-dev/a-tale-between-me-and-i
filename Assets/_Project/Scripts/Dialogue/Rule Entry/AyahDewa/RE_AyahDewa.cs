using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Event;
using ATBMI.Enum;

namespace ATBMI.Dialogue
{
    public class RE_AyahDewa : RuleEntry
    {
        #region Dialogue Rules Parameters
        [Space(20)]
        [Header("Dialogue Rules")]
        // [SerializeField] private bool isRunning;
        // public bool IsRunning => isRunning;
        #endregion

        #region Dialogue Text Assets
        [Space(20)]
        [Header("Dialogue Text Assets")]
        // public TextAsset onTalk;

        [Space(10)]
        public TextAsset onTalkedTo_JalanBali_RunOrWalkWOInteract; // NPC Menginisialisasi percakapan
        public bool isOnce_JalanBali_RunOrWalkWOInteract;

        [Space(10)]
        public TextAsset JalanBali_RunWithInteraction; // Menginialisasi percakapan ke NPC
        public bool isOnce_JalanBali_RunWithInteraction;

        public TextAsset JalanBali_WalkWithInteraction;
        public bool isOnce_JalanBali_WalkWithInteraction;
        #endregion

        #region Backing fields for the 'isOnce' properties
        [HideInInspector] public bool isOnce01;
        [HideInInspector] public bool isOnce02;
        [HideInInspector] public bool isOnce03;
        #endregion

        // if dialogue trigger after player entering object/npc area || NPC Inisiasi percakapan
        // public override void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         isPlayerInRange = true;

        //         foreach (var rule in autoTriggerRules)
        //         {
        //             if (rule.Evaluate(this))
        //             {
        //                 rule.Execute(this);
        //                 break; // Execute only the first valid rule
        //             }
        //         }
        //     }
        // }

        // // if dialogue trigger after player exiting object/npc area
        // // public override void OnTriggerExit2D(Collider2D other)
        // // {
        // //     if (other.CompareTag("Player"))
        // //     {
        // //         isPlayerInRange = false;
        // //     }
        // // }

        // // Player Inisialisasi percakapan (Dialog)
        // public override void OnEnterDialogue(TextAsset defaultDialogue)
        // {
        //     if (!isDialogueAboutToStart && isPlayerInRange)
        //     {
        //         foreach (var rule in manualTriggerRules)
        //         {
        //             if (rule.Evaluate(this))
        //             {
        //                 rule.Execute(this);
        //                 break; // Execute only the first valid rule
        //             }
        //         }

        //         // TODO: change to default dialogue (use rules if default dialogue > 1, eg. default dialogue ch1, ch2, ch3...)
        //         base.EnterDialogue(this, defaultDialogue);
        //     }
        // }

        // // Player Inisialisasi percakapan (Item)
        // // public override void OnEnterItemDialogue(TextAsset itemDialogue)
        // // {
        // //     if (!isDialogueAboutToStart && isPlayerInRange)
        // //     {
        // //         base.EnterDialogue(this, itemDialogue);
        // //     }
        // // }

        // // public override void EnterDialogueWithInkJson(TextAsset InkJson)
        // // {
        // //     base.EnterDialogueWithInkJson(InkJson);
        // // }

        // public override void IsPlayerRun(bool isRunning)
        // {
        //     this.isRunning = isRunning;
        // }
    }
}
