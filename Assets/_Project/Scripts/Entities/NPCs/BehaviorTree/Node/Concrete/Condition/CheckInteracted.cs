using ATBMI.Dialogue;
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
            return interact.IsInteracting || DialogueManager.Instance.IsDialoguePlaying?
                NodeStatus.Success :
                NodeStatus.Failure;
        }
    }
}