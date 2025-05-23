using ATBMI.Cutscene;
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
            return interact.IsInteracting ? NodeStatus.Success : NodeStatus.Failure;
            // return interact.IsInteracting || DialogueManager.Instance.IsDialoguePlaying 
            //                               || CutsceneManager.Instance.IsCutscenePlaying
            //     ? NodeStatus.Success 
            //     : NodeStatus.Failure;
        }
    }
}