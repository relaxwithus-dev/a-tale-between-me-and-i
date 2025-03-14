namespace ATBMI.Entities.NPCs
{
    public class TaskIdle : LeafWeight
    {
        private readonly CharacterAI character;

        public TaskIdle(CharacterAI character)
        {
            this.character = character;
            InitFactors(plan: 1f, risk: 0f, timeRange: (0.1f, 0.1f));
        }
        
        public override NodeStatus Evaluate()
        {
            character.ChangeState(CharacterState.Idle);
            return NodeStatus.Success;
        }
    }
}