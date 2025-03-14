using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class CheckInAction : LeafWeight
    {
        private readonly CharacterInteract interact;
        private readonly bool isAction;

        public CheckInAction(CharacterInteract interact, bool isAction)
        {
            this.interact = interact;
            this.isAction = isAction;
            
            InitFactors(plan: 0f, risk: 0f, timeRange:(0f, 0f));
        }
        
        public override NodeStatus Evaluate()
        {
            interact.WhenAction(isAction);
            return NodeStatus.Success;
        }
    }
}