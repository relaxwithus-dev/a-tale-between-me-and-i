namespace ATBMI.Entities.NPCs
{
    public class CheckInteractCount : LeafWeight
    {
        private readonly int interactCount;
        
        public CheckInteractCount(int interactCount)
        {
            this.interactCount = interactCount; 
            
            InitFactors(planning: 0, risk: 0, timeRange: (0, 0));
        }

        public override NodeStatus Evaluate()
        {
            return base.Evaluate();
        }
    }
}