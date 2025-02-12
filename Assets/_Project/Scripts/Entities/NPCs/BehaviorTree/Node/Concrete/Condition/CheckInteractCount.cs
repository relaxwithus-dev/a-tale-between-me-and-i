using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckInteractCount : LeafWeight
    {
        private readonly EmoTrees emoTrees;
        private readonly int interactTarget;
        
        public CheckInteractCount(EmoTrees emoTrees, int interactTarget)
        {
            this.emoTrees = emoTrees;
            this.interactTarget = interactTarget; 
            
            InitFactors(planning: 0, risk: 0, timeRange: (0, 0));
        }
        
        public override NodeStatus Evaluate()
        {
            if (emoTrees.InteractCount == interactTarget)
            {
                Debug.Log($"Execute: Interact Count {interactTarget} Success");
                return NodeStatus.Success;
            }
            
            Debug.Log($"Execute: Interact Count {interactTarget} Failure");
            return NodeStatus.Failure;
        }
    }
}