using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class TaskPush : TaskForceBase
    {
        private bool _isPushing;
        private bool _isDonePushing;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsPush = new()
        {
            { Emotion.Joy, (1, 0.8f, (0.2f, 0.5f)) },
            { Emotion.Trust, (1, 0.8f, (0.4f, 0.9f)) },
            { Emotion.Fear, (1, 0.8f, (0.6f, 1f)) },
            { Emotion.Surprise, (1, 0.6f, (0.5f, 0.8f)) },
            { Emotion.Sadness, (1, 0.8f, (0.5f, 0.9f)) },
            { Emotion.Disgust, (1, 0.8f, (0.5f, 0.9f)) },
            { Emotion.Anger, (1, 0.8f, (0.3f, 1f)) },
            { Emotion.Anticipation, (1, 0.8f, (0.2f, 0.2f)) }
        };
        
        // Constructor
        public TaskPush(CharacterAI character, CharacterAnimation animation, float force, float delay) 
            : base(character, animation, force, delay)
        {
            OverrideEmotionFactors(_factorsPush);
        }
        
        // Core
        protected override NodeStatus PerformForce()
        {
            if (_isPushing)
                return NodeStatus.Running;

            if (_isDonePushing)
                return NodeStatus.Success;
            
            var direction = (player.transform.position - character.transform.position).normalized;
            character.StartCoroutine(PushRoutine(direction));
            return NodeStatus.Success;
        }
        
        private IEnumerator PushRoutine(Vector3 direction)
        {
            _isPushing = true;
            animation.TrySetAnimationState(StateTag.PUSH_STATE);

            yield return new WaitForSeconds(0.15f);
            player.StopMovement();
            player.PlayerRb.AddForce(direction * force, ForceMode2D.Impulse);
            player.StartCoroutine(WhenDoneForce());
            
            InteractEvent.RestrictedEvent(true);
            _isDonePushing = true;
        }

        protected override void Reset()
        {
            base.Reset();
            _isPushing = false;
            _isDonePushing = false;
        }
    }
}