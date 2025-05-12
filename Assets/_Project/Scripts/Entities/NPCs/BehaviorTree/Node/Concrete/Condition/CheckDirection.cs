using ATBMI.Entities.Player;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckDirection : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly bool isDifferentDir;

        public CheckDirection(CharacterAI character, bool isDifferentDir = false)
        {
            this.character = character;
            this.isDifferentDir = isDifferentDir;
        }

        public override NodeStatus Evaluate()
        {
            if (GetData(TARGET_KEY) is not Transform target)
                return LogFailure();

            if (!target.TryGetComponent<PlayerController>(out var player))
                return LogFailure();

            var facingSameDirection = player.IsFacingRight == character.IsFacingRight;
            if ((isDifferentDir && !facingSameDirection) || (!isDifferentDir && facingSameDirection))
                return LogSuccess();
            
            return LogFailure();
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