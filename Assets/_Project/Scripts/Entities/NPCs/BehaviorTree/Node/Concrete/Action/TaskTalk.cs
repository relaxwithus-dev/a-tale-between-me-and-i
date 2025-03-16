using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskTalk : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterState state;
        private readonly string dialogueText;
        
        private bool _isDialogueEnded;
        
        public TaskTalk(CharacterAI character, string dialogueText)
        {
            this.character = character;
            this.dialogueText = dialogueText;
            
            InitFactors(plan: 1f, risk: 0.2f, timeRange: (2f, 4.5f));
        }
        
        public TaskTalk(CharacterAI character, CharacterState state, string dialogueText)
        {
            this.character = character;
            this.dialogueText = dialogueText;
            this.state = state;
            
            InitFactors(plan: 1f, risk: 0.2f, timeRange: (0.5f, 1f));
        } 
        
        public override NodeStatus Evaluate()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isDialogueEnded)
            {
                return NodeStatus.Success;
            }
            
            Debug.Log($"Execute: TaskDialogue( {dialogueText} )");
            var targetState = state is CharacterState.Idle ? CharacterState.Talk : state;
            character.ChangeState(targetState);
            _isDialogueEnded = true;
            return NodeStatus.Running;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _isDialogueEnded = false;
        }
    }
}