using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskIdle : LeafWeight
    {
        private readonly CharacterAI character;

        public TaskIdle(CharacterAI character)
        {
            this.character = character;
            InitFactors(planning: 1f, risk: 0f, timeRange: (0, 0));
        }
        
        public override NodeStatus Evaluate()
        {
            if (character.State == CharacterState.Idle)
                return NodeStatus.Success;
            
            Debug.Log("Execute Success: Task Idle");
            character.ChangeState(CharacterState.Idle);
            return NodeStatus.Running;
        }
    }
}