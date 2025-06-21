using UnityEngine;

namespace ATBMI.Dialogue
{
    public abstract class DialogueRuleBase : ScriptableObject
    {
        public abstract bool Evaluate(RuleEntry context);
        public abstract void Execute(RuleEntry context);
    }

    public abstract class DialogueRuleBase<T> : DialogueRuleBase where T : RuleEntry
    {
        public override bool Evaluate(RuleEntry context) => Evaluate((T)context);
        public override void Execute(RuleEntry context) => Execute((T)context);

        public abstract bool Evaluate(T context);
        public abstract void Execute(T context);
    }
}
