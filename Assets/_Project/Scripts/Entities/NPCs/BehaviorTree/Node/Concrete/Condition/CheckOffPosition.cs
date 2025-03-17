using ATBMI.Entities.NPCs;
using ATBMI.Entities.Player;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckOffPosition : LeafWeight
    {
        private readonly CharacterAI character;

        public CheckOffPosition(CharacterAI character)
        {
            this.character = character;
        }

        public override NodeStatus Evaluate()
        {
            if (GetData(TARGET_KEY) is not Transform target)
                return LogFailure();
            
            var targetIsOnRight = target.position.x > character.transform.position.x;
            var characterIsFacingTarget = character.IsFacingRight == targetIsOnRight;
            
            return characterIsFacingTarget ? LogSuccess() : LogFailure();
        }
        
        private NodeStatus LogFailure()
        {
            Debug.LogWarning("Execute Failure: CheckSameDirection");
            return NodeStatus.Failure;
        }

        private NodeStatus LogSuccess()
        {
            Debug.Log("Execute Success: CheckSameDirection");
            return NodeStatus.Success;
        }
    }
}
