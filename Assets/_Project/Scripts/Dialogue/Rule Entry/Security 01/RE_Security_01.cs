using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Dialogue
{
    public class RE_Security_01 : RuleEntry
    {
        #region Dialogue Rules Parameters
        [Space(20)]
        [Header("Dialogue Rules")]
        [SerializeField] private bool isAfterExplosion;
        [SerializeField] private bool isRunning;
        [SerializeField] private int visitedCount;
        [SerializeField] private bool isAfterGettingItem;
        public bool IsAfterExplosion => isAfterExplosion;
        public bool IsRunning => isRunning;
        public int VisitedCount
        {
            get => visitedCount;
            set => visitedCount = value;
        }
        public bool IsAfterGettingItem => isAfterGettingItem;
        #endregion

        #region Dialogue Text Assets
        [Space(20)]
        [Header("Dialogue Text Assets")]
        public TextAsset onTalk;

        [Space(10)]
        public TextAsset onTalkedTo_AfterExplosion_WithRunning;
        public bool isOnce_AfterExplosion_WithRunning;

        [Space(10)]
        public TextAsset onTalkedTo_AfterExplosion_WithRunning_Visited_01;
        public bool isOnce_AfterExplosion_WithRunning_Visited_01;

        public TextAsset onTalkedTo_AfterExplosion_WithRunning_Visited_02;
        public bool isOnce_AfterExplosion_WithRunning_Visited_02;

        public TextAsset onTalkedTo_AfterExplosion_WithRunning_Visited_03;
        public bool isOnce_AfterExplosion_WithRunning_Visited_03;

        [Space(10)]
        public TextAsset onTalk_AfterExplosion_WithWalking_Visited_01;
        public bool isOnce_AfterExplosion_WithWalking_Visited_01;

        public TextAsset onTalk_AfterExplosion_WithWalking_Visited_02;
        public bool isOnce_AfterExplosion_WithWalking_Visited_02;

        [Space(10)]
        public TextAsset onTalkedTo_AfterGettingAnItem;
        public bool isOnce_AfterGettingItem;

        public TextAsset onTalk_AfterGettingAKey;
        public bool isOnce_AfterGettingKey;

        public TextAsset onTalk_AfterGettingARock;
        public bool isOnce_AfterGettingRock;
        #endregion

        #region Backing fields for the 'isOnce' properties
        [HideInInspector] public bool isOnce01;
        [HideInInspector] public bool isOnce02;
        [HideInInspector] public bool isOnce03;
        [HideInInspector] public bool isOnce04;
        [HideInInspector] public bool isOnce05;
        [HideInInspector] public bool isOnce06;
        [HideInInspector] public bool isOnce07;
        [HideInInspector] public bool isOnce08;
        [HideInInspector] public bool isOnce09;
        #endregion

        private void Awake()
        {
            // TODO: change the method of getting this script
            // playerInputHandler = FindObjectOfType<PlayerInputHandler>();
            // interactManager = GetComponent<InteractManager>();

            // visualCue = GetComponentInChildren<VisualCue>();
            // npc = transform.parent.gameObject.GetComponent<NPC>();

            InitializeRules();
        }

        private void InitializeRules()
        {
            // dialogueRules = new List<IDialogueRule<RE_Security_01>>
            // {
            //     new OnTalk_ExplosionRunningRule(),
            //     new OnTalk_ExplosionWalkingRule(),
            //     new OnTalk_GettingItemRule()
            // };

            // triggerRules = new List<IDialogueRule<RE_Security_01>>
            // {
            //     new OnTalkedTo_ExplosionRunningRule(),
            //     new OnTalkedTo_GettingItemRunningRule()
            // };

            // Sort rules by priority
            // dialogueRules = dialogueRules.OrderBy(rule => rule.RulePriority).ToList();
            // triggerRules = triggerRules.OrderBy(rule => rule.RulePriority).ToList();
        }

        private void Start()
        {
            // if (visualCue != null)
            // {
            //     isVisualCueExists = true;
            //     visualCue.DeactivateVisualCue();
            // }
            // else
            // {
            //     isVisualCueExists = false;
            // }

            isPlayerInRange = false;
            isDialogueAboutToStart = false;

            visitedCount = 0;
        }

        // if dialogue trigger after player entering object/npc area
        public override void OnTriggerEnter2D(Collider2D other)
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

        // if dialogue trigger after player exiting object/npc area
        public override void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
            }
        }

        public override void EnterDialogue()
        {
            if (!isDialogueAboutToStart && isPlayerInRange)
            {
                foreach (var rule in manualTriggerRules)
                {
                    if (rule.Evaluate(this))
                    {
                        rule.Execute(this);
                        break; // Execute only the first valid rule
                    }
                }

                if (!isDialogueAboutToStart)
                {
                    isDialogueAboutToStart = true;
                    PlayerEvents.MoveToPlayerEvent(this, onTalk, playerEntryPoint.position.x, transform.position.x, npc.isFacingRight);
                }
            }
        }

        public override void EnterDialogueWithInkJson(TextAsset InkJson)
        {
            base.EnterDialogueWithInkJson(InkJson);
        }
    }
}
