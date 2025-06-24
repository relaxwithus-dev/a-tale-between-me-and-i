using UnityEngine;

namespace ATBMI.Dialogue
{
    public class RE_Security_01 : RuleEntry
    {
        public override void InitializeRuleEntry()
        {
            base.InitializeRuleEntry();

            // You can set any default state here if needed
            isAfterExplosion = false;
            isAfterGettingItem = false;
        }
    }
}
