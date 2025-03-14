using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class CheckInteracted : Leaf
    {
        private readonly CharacterInteract interact;

        public CheckInteracted(CharacterInteract interact)
        {
            this.interact = interact;
        }
        
        public override NodeStatus Evaluate()
        {
            return interact.IsInteracting ?
                NodeStatus.Success :
                NodeStatus.Failure;
        }
    }
}