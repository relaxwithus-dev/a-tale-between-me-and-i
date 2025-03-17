using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class CheckInAction : LeafWeight
    {
        private readonly bool isAction;
        
        public CheckInAction(bool isAction)
        {
            this.isAction = isAction;
        }
        
        public override NodeStatus Evaluate()
        {
            InteractEvent.RestrictedEvent(isAction);
            return NodeStatus.Success;
        }
    }
}