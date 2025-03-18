using System.Collections.Generic;
using ATBMI.Interaction;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskPush : TaskForceBase
    {
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsPush = new()
        {
            { Emotion.Joy, (1, 0.8f, (0.2f, 0.5f)) },
            { Emotion.Trust, (1, 0.8f, (0.4f, 0.9f)) },
            { Emotion.Fear, (1, 0.8f, (0.6f, 1f)) },
            { Emotion.Surprise, (1, 0.6f, (0.5f, 0.8f)) },
            { Emotion.Sadness, (1, 0.8f, (0.5f, 0.9f)) },
            { Emotion.Disgust, (1, 0.8f, (0.5f, 0.9f)) },
            { Emotion.Anger, (1, 0.8f, (0.3f, 1f)) },
            { Emotion.Anticipation, (1, 0.8f, (0.5f, 0.9f)) }
        };
        
        // Constructor
        public TaskPush(CharacterAI character, float force, float delay) : base(character, force, delay)
        {
            OverrideEmotionFactors(_factorsPush);
        }
        
        // Core
        protected override NodeStatus PerformForce()
        {
            Vector2 direction = (player.transform.position - character.transform.position).normalized;
            
            player.StopMovement();
            player.PlayerRb.AddForce(direction * force, ForceMode2D.Impulse);
            player.StartCoroutine(WhenDoneForce());
            
            InteractEvent.RestrictedEvent(true);
            return NodeStatus.Success;
        }
    }
}