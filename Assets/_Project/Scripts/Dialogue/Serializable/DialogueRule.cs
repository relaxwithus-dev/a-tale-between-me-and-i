using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Dialogue
{
    [Serializable]
    public class DialogueRule
    {
        public List<RuleFlag> booleanConditions;
        public List<NumericCondition> numericConditions;
        public TextAsset dialogue;
        public bool isOnce;
        [HideInInspector] public bool hasExecuted;
    }
}
