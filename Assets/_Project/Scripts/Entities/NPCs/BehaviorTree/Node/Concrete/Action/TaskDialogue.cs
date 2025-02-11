using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskDialogue : LeafWeight
    {
        private readonly string dialogueText;
        private readonly CharacterAI characterAI;
        
        public TaskDialogue(CharacterAI characterAI, string dialogueText)
        {
            this.characterAI = characterAI;
            this.dialogueText = dialogueText;
            
            InitFactors(planning: 1f, risk: 0f, timeRange: (1, 2.5f));
        }
        
        public override NodeStatus Evaluate()
        {
            Debug.Log($"Execute: TaskDialogue({dialogueText})");
            return NodeStatus.Success;
        }
    }
}