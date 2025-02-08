using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class CheckInteract : Leaf
    {
        private readonly CharacterInteract characterInteract;

        public CheckInteract(CharacterInteract characterInteract)
        {
            this.characterInteract = characterInteract;
        }
        
        public override NodeStatus Evaluate()
        {
            var isInteracting = characterInteract.IsInteracting 
                ? NodeStatus.Success 
                : NodeStatus.Failure;
            
            return isInteracting;
        }
    }
}