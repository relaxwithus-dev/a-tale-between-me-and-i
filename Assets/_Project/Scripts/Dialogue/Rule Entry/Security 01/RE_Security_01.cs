using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Event;
using ATBMI.Enum;

namespace ATBMI.Dialogue
{
    public class RE_Security_01 : RuleEntry
    {
        #region Dialogue Rules Parameters
        [Space(20)]
        [Header("Dialogue Rules")]
        [SerializeField] private bool isAfterExplosion;
        // [SerializeField] private bool isRunning;
        [SerializeField] private int visitedCount;
        [SerializeField] private bool isAfterGettingItem;
        public bool IsAfterExplosion => isAfterExplosion;
        // public bool IsRunning => isRunning;
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
        // public TextAsset onTalk;

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

        private void Start()
        {
            InitializeRuleEntry();
        }

        public override void InitializeRuleEntry()
        {
            base.InitializeRuleEntry();

            visitedCount = 0;
        }
    }
}
