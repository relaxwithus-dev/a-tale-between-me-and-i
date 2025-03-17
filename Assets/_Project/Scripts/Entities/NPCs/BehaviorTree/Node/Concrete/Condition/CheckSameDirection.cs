using ATBMI.Entities.Player;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckSameDirection : LeafWeight
    {
        private readonly CharacterAI character;

        public CheckSameDirection(CharacterAI character)
        {
            this.character = character;
        }

        public override NodeStatus Evaluate()
        {
            if (GetData(TARGET_KEY) is not Transform target)
                return LogFailure();

            if (!target.TryGetComponent<PlayerController>(out var player))
                return LogFailure();

            if (player.IsRight && character.IsFacingRight)
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