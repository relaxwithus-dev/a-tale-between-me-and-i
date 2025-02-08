using UnityEngine;
using ATBMI.Enum;

namespace ATBMI.Entities.NPCs
{
    public class CheckInteractPassed : Leaf
    {
        // TODO: Complete mekanik node ini bruh
        private readonly CharacterAI character;
        private readonly CharacterTraits traits;
        private readonly Vector2 direction;
        private readonly bool isRunning;
        
        public CheckInteractPassed(CharacterAI character, CharacterTraits traits, Vector2 direction, bool isRunning)
        {
            this.character = character;
            this.traits = traits;
            this.direction = direction;
            this.isRunning = isRunning;
        }
        
        public override NodeStatus Evaluate()
        {
            // Logic here
            if (true)
            {
                var action = isRunning ? InteractAction.Run : InteractAction.Walk;
                traits.InfluenceTraits(action);
                return NodeStatus.Success;
            }
        }
    }
}