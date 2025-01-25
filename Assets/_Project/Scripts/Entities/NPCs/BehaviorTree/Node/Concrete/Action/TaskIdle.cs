namespace ATBMI.Entities.NPCs
{
    public class TaskIdle : Node
    {
        private readonly CharacterAI characterAI;
        
        public TaskIdle(CharacterAI characterAI)
        {
            this.characterAI = characterAI;
        }
        
        public override NodeStatus Evaluate()
        {
            if (characterAI.State == CharacterState.Idle)
                return NodeStatus.Success;
            
            characterAI.ChangeState(CharacterState.Idle);
            return NodeStatus.Running;
        }
    }
}