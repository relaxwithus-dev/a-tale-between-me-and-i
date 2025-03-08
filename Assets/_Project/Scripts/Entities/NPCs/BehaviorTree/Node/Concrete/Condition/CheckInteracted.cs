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
            if (interact.IsInteracting)
            {
                Debug.Log("Execute Success: CheckInteracted");
                return NodeStatus.Success;
            }
            
            Debug.LogWarning("Execute Failure: CheckInteracted");
            return NodeStatus.Failure;
        }
    }
}