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

        private bool isDialogueAboutToStart;
        private bool isRunning;
        private bool isPlayerInRange;
        private bool isExecuted;

        public bool IsDialogueAboutToStart { get; set; }
        public bool IsPlayerInRange => isPlayerInRange;
        public bool IsExecuted => isExecuted;
        public bool IsRunning => isRunning;

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
            DialogueEvents.OnEnterDialogue += OnEnterDialogue;
            DialogueEvents.OnEnterItemDialogue += OnEnterItemDialogue;
            DialogueEvents.OnExitDialogue += ExitDialogue;
            DialogueEvents.PlayerRun += IsPlayerRun;
        }
        
        private void OnDisable()
        {
            DialogueEvents.OnEnterDialogue -= OnEnterDialogue;
            DialogueEvents.OnEnterItemDialogue -= OnEnterItemDialogue;
            DialogueEvents.OnExitDialogue -= ExitDialogue;
            DialogueEvents.PlayerRun -= IsPlayerRun;
        }
        
        public virtual void InitializeRuleEntry()
        {
            isPlayerInRange = false;
            isDialogueAboutToStart = false;
        }
        
        public void ExitDialogue()
        {
            isExecuted = false;
        }
        
        public void EnterDialogueWithInkJson(TextAsset InkJson)
        {
            DialogueManager.Instance.EnterDialogueMode(InkJson);
            isDialogueAboutToStart = false;
            isExecuted = true;
        }

        public void OnEnterItemDialogue(TextAsset itemDialogue)
        {
            if (!isDialogueAboutToStart && IsPlayerInRange)
            {
                EnterDialogue(this, itemDialogue);
            }
        }
        
        public void EnterDialogue(RuleEntry ruleEntry, TextAsset dialogue)
        {
            if (!isDialogueAboutToStart)
            {
                isDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(ruleEntry, dialogue, ruleEntry.playerEntryPoint.position.x, ruleEntry.transform.position.x, ruleEntry.npc.IsFacingRight);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;

                foreach (var rule in autoTriggerRules)
                {
                    if (rule.Evaluate(this))
                    {
                        rule.Execute(this);
                        break; // Execute only the first valid rule
                    }
                }
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
            }
        }

        public void OnEnterDialogue(TextAsset defaultDialogue)
        {
            if (!isDialogueAboutToStart && IsPlayerInRange)
            {
                foreach (var rule in manualTriggerRules)
                {
                    if (rule.Evaluate(this))
                    {
                        rule.Execute(this);
                        break; // Execute only the first valid rule
                    }
                }

                // TODO: change to default dialogue (use rules if default dialogue > 1, eg. default dialogue ch1, ch2, ch3...)
                EnterDialogue(this, defaultDialogue);
            }
        }

        public void IsPlayerRun(bool isRunning)
        {
            this.isRunning = isRunning;
        }
    }
}
