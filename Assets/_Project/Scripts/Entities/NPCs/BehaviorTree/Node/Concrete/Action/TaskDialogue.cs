using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskDialogue : Leaf
    {
        private readonly string dialogueText;
        private readonly CharacterAI characterAI;
        
        public TaskDialogue(CharacterAI characterAI, string dialogueText)
        {
            this.characterAI = characterAI;
            this.dialogueText = dialogueText;
        }
        
        public override NodeStatus Evaluate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log(dialogueText);
                return NodeStatus.Success;
            }
            
            characterAI.ChangeState(CharacterState.Talk);
            Debug.Log("press space to continue");
            return NodeStatus.Running;
        }
    }
}