using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Dialogue
{
    public class RE_Raya : RuleEntry
    {
        #region Dialogue Rules Parameters

        [Space(20)]
        [Header("Dialogue Rules")]
        [SerializeField] private int visitedCount;
        // [SerializeField] private bool isRunning;
        // public bool IsRunning => isRunning;
        #endregion
        #region Dialogue Text Assets
        [Space(20)]
        [Header("Dialogue Text Assets")]
        public TextAsset onTalk;

        #endregion

        // Start is called before the first frame update
        void Start()
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
