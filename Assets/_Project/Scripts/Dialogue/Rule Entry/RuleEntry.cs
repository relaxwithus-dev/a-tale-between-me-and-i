using ATBMI.Entities.NPCs;
using System;
using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI.Dialogue
{
    public abstract class RuleEntry : MonoBehaviour
    {
        [Header("Params")]
        public CharacterAI npc;
        public Transform playerEntryPoint;
        public Animator emoteAnimator;
        public bool isPlayerInRange;
        public bool isDialogueAboutToStart;

        #region Dialogue Rules
        [Space(20)]
        [Header("On Talk (Manual Trigger Dialogue Rules)")]
        [Tooltip("Rule Priotiry determined by these dialogue rules index")]
        public List<DialogueRuleBase> manualTriggerRules;

        [Space(5)]
        [Header("On Talked To (Auto Trigger Dialogue Rules)")]
        [Tooltip("Rule Priotiry determined by these dialogue rules index")]
        public List<DialogueRuleBase> autoTriggerRules;
        #endregion

        private void OnEnable()
        {
            DialogEvents.EnterDialogue += EnterDialogue;
        }

        private void OnDisable()
        {
            DialogEvents.EnterDialogue -= EnterDialogue;
        }

        public abstract void OnTriggerEnter2D(Collider2D other);
        public abstract void OnTriggerExit2D(Collider2D other);
        public abstract void EnterDialogue();
        
        public virtual void EnterDialogueWithInkJson(TextAsset InkJson)
        {
            DialogueManager.Instance.EnterDialogueMode(InkJson, emoteAnimator);

            isDialogueAboutToStart = false;
        }
    }
}
