using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class CheckInAction : LeafWeight
    {
        private readonly bool isAction;
        
        public CheckInAction(bool isAction)
        {
            this.isAction = isAction;
            
            InitFactors(plan: 0f, risk: 0f, timeRange:(0f, 0f));
        }
        
        public override NodeStatus Evaluate()
        {
            InteractEvent.RestrictedEvent(isAction);
            return NodeStatus.Success;
        }
    }
}