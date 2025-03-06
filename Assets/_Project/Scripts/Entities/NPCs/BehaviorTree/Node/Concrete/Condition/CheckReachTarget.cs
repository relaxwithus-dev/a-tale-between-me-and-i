using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckReachTarget : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly Vector3 targetPosition;
        
        public CheckReachTarget(CharacterAI character, Transform targetPoint)
        {
            this.character = character;
            targetPosition = new Vector3(targetPoint.position.x,
                character.transform.position.y,
                character.transform.position.z);
            
            InitFactors(plan: 0f, risk: 0f, timeRange: (0, 0));
        }
        
        public override NodeStatus Evaluate()
        {
            var distance = Vector3.Distance(character.transform.position, targetPosition);
            if (distance <= 0.01f)
            {
                Debug.Log("Execute Failure: Check Reach Target");
                return NodeStatus.Failure;
            }
            
            Debug.Log("Execute Success: Check Reach Target");
            return NodeStatus.Success;
        }
    }
}