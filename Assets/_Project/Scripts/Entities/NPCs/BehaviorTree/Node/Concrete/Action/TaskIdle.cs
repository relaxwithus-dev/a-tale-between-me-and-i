namespace ATBMI.Entities.NPCs
{
    public class TaskIdle : LeafWeight
    {
        private readonly CharacterAI characterAI;

        public TaskIdle(CharacterAI characterAI)
        {
            this.characterAI = characterAI;
            InitFactors(planning: 1f, risk: 0f, timeRange: (0, 0));
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