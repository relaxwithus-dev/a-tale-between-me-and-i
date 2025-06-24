using System.Collections.Generic;
using UnityEngine;
using ATBMI.Entities.NPCs;
using ATBMI.Gameplay.Event;
using Sirenix.OdinInspector;

namespace ATBMI.Dialogue
{
    public class RuleEntry : MonoBehaviour
    {
        [Header("NPC & Dialogue")]
        public CharacterAI npc;
        public Transform playerEntryPoint;
        
        [PropertySpace(15)] public List<DialogueRule> rules;

        [PropertySpace(15)]
        [LabelText("Enable Debug Mode")] public bool debugMode;

        // Debug Group – shown only if debugMode is true
        [BoxGroup(), ShowIf("debugMode"), LabelText("After Explosion")] public bool isAfterExplosion;

        [BoxGroup(), ShowIf("debugMode"), LabelText("After Getting Item")] public bool isAfterGettingItem;

        [BoxGroup(), ShowIf("debugMode"), LabelText("Is Running")] public bool isRunning;

        [BoxGroup(), ShowIf("debugMode"), LabelText("Visited Count")] public int visitedCount;

        // Internal state
        private bool isPlayerInRange;
        private bool isDialogueAboutToStart;
        private bool isExecuted;

        #region Unity Events
        protected virtual void OnEnable()
        {
            DialogueEvents.OnEnterDialogue += OnEnterDialogue;
            DialogueEvents.OnEnterItemDialogue += OnEnterItemDialogue;
            DialogueEvents.OnExitDialogue += ExitDialogue;
            DialogueEvents.PlayerRun += SetPlayerRun;
        }

        protected virtual void OnDisable()
        {
            DialogueEvents.OnEnterDialogue -= OnEnterDialogue;
            DialogueEvents.OnEnterItemDialogue -= OnEnterItemDialogue;
            DialogueEvents.OnExitDialogue -= ExitDialogue;
            DialogueEvents.PlayerRun -= SetPlayerRun;
        }
        #endregion

        public virtual void InitializeRuleEntry()
        {
            isPlayerInRange = false;
            isDialogueAboutToStart = false;
            visitedCount = 0;
        }

        private void SetPlayerRun(bool value) => isRunning = value;

        private void ExitDialogue() => isExecuted = false;

        private void OnEnterItemDialogue(TextAsset itemDialogue)
        {
            if (!isDialogueAboutToStart && isPlayerInRange)
                EnterDialogue(this, itemDialogue);
        }

        private void OnEnterDialogue(TextAsset defaultDialogue)
        {
            if (!isDialogueAboutToStart && isPlayerInRange)
            {
                DialogueRule rule = GetValidRule();
                if (rule != null)
                    EnterDialogueWithInkJson(rule.dialogue);
                else
                    EnterDialogueWithInkJson(defaultDialogue);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;

                DialogueRule rule = GetValidRule();
                if (rule != null)
                    EnterDialogueWithInkJson(rule.dialogue);
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                isPlayerInRange = false;
        }

        public void EnterDialogueWithInkJson(TextAsset inkJson)
        {
            DialogueManager.Instance.EnterDialogueMode(inkJson);
            isDialogueAboutToStart = false;
            isExecuted = true;
        }

        public void EnterDialogue(RuleEntry ruleEntry, TextAsset dialogue)
        {
            if (!isDialogueAboutToStart)
            {
                isDialogueAboutToStart = true;
                PlayerEvents.MoveToPlayerEvent(ruleEntry, dialogue, playerEntryPoint.position.x, transform.position.x, npc.IsFacingRight);
            }
        }

        #region Rule Evaluation

        protected DialogueRule GetValidRule()
        {
            foreach (var rule in rules)
            {
                if (rule.isOnce && rule.hasExecuted)
                    continue;

                bool allBooleansMatch = rule.booleanConditions.TrueForAll(flag => CheckBooleanCondition(flag));
                bool allNumericsMatch = rule.numericConditions.TrueForAll(cond => EvaluateNumericCondition(cond));

                if (allBooleansMatch && allNumericsMatch)
                {
                    rule.hasExecuted = true;

                    if (RuleContainsVisitedCountCondition(rule))
                        visitedCount++;

                    return rule;
                }
            }

            return null;
        }

        private bool RuleContainsVisitedCountCondition(DialogueRule rule)
        {
            foreach (var cond in rule.numericConditions)
            {
                if (cond.type == NumericConditionType.VisitedCount &&
                    (cond.comparison == ComparisonOperator.Equal || cond.comparison == ComparisonOperator.GreaterThan))
                    return true;
            }
            return false;
        }

        private bool EvaluateNumericCondition(NumericCondition condition)
        {
            int currentValue = GetNumericValue(condition.type);
            return condition.comparison switch
            {
                ComparisonOperator.Equal => currentValue == condition.value,
                ComparisonOperator.GreaterThan => currentValue > condition.value,
                ComparisonOperator.LessThan => currentValue < condition.value,
                _ => false
            };
        }

        protected virtual int GetNumericValue(NumericConditionType type)
        {
            return type switch
            {
                NumericConditionType.VisitedCount => visitedCount,
                _ => 0
            };
        }

        protected virtual bool CheckBooleanCondition(RuleFlag flag)
        {
            return flag switch
            {
                RuleFlag.IsAfterExplosion => isAfterExplosion,
                RuleFlag.IsAfterGettingItem => isAfterGettingItem,
                RuleFlag.IsRunning => isRunning,
                _ => false
            };
        }

        #endregion
    }
}
