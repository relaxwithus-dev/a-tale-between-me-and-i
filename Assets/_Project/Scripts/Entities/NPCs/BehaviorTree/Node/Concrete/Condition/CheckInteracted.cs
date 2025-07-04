using ATBMI.Cutscene;
using ATBMI.Dialogue;
using ATBMI.Interaction;
using UnityEngine;

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
            return interact.IsInteracting || CutsceneManager.Instance.IsCutscenePlaying
                ? NodeStatus.Success 
                : NodeStatus.Failure;
        }
    }
}