// using UnityEngine;

// namespace ATBMI.Dialogue
// {
//     [CreateAssetMenu(fileName = "OnTalkedTo_GettingItemRunningRule", menuName = "Data/Dialogue Rules/Security_01/OnTalkedTo_GettingItemRunningRule")]
//     public class OnTalkedTo_GettingItemRunningRule : DialogueRuleBase<RE_Security_01>
//     {
//         // public int RulePriority => (int)RulePrioritySecurity_01.OnTalkedTo_GettingItemRunningRule;

//         public override bool Evaluate(RE_Security_01 context)
//         {
//             return context.IsPlayerInRange && context.IsAfterGettingItem && context.IsRunning && !context.isOnce07;
//         }

//         public override void Execute(RE_Security_01 context)
//         {
//             DialogueManager.Instance.EnterDialogueMode(context.onTalkedTo_AfterGettingAnItem);
//             if (context.isOnce_AfterGettingItem)
//             {
//                 context.isOnce07 = true;
//             }
//         }
//     }
// }
