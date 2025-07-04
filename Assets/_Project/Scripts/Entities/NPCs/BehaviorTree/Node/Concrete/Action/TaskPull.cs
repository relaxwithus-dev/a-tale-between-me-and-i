using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskPull : TaskForceBase
    {
        private readonly float holdForce = 0.9f;
        private readonly float holdTime = 0.3f;
        
        private bool _isPull;
        private bool _isDonePull;
        
        private bool _isHolding;
        private float _currentHoldTime;
        private Vector3 _holdDirection;
        private Vector3 _pullDirection;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsPull = new()
        {
            { Emotion.Joy, (1, 0.7f, (0.3f, 1f)) },
            { Emotion.Trust, (1, 0.7f, (0.8f, 1.5f)) },
            { Emotion.Fear, (1, 0.7f, (0.8f, 1.4f)) },
            { Emotion.Surprise, (1, 0.6f, (0.4f, 1.2f)) },
            { Emotion.Sadness, (1, 0.7f, (0.6f, 1.5f)) },
            { Emotion.Disgust, (1, 0.7f, (0.6f, 1.5f)) },
            { Emotion.Anger, (1, 0.7f, (0.3f, 0.6f)) },
            { Emotion.Anticipation, (1, 0.7f, (0.6f, 1.5f)) }
        };

        // Constructor
        public TaskPull(CharacterAI character, CharacterAnimation animation, float force, float delay) 
            : base(character, animation, force, delay)
        {
            OverrideEmotionFactors(_factorsPull);
        }
        
        // Core
        protected override NodeStatus PerformForce()
        {
            if (_isPull)
                return NodeStatus.Running;
            
            if (_isDonePull)
                return NodeStatus.Success;
            
            if (_currentHoldTime < holdTime)
            {
                if (!_isHolding)
                {
                    InitiateDirection();
                    HoldTarget();
                }
                
                _currentHoldTime += Time.deltaTime;
                return NodeStatus.Running;
            }
            
            character.StartCoroutine(PullRoutine());
            return NodeStatus.Running;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _isHolding = false;
            _currentHoldTime = 0f;
            _pullDirection = Vector3.zero;
        }
        
        private void InitiateDirection()
        {
            if (_pullDirection != Vector3.zero && _holdDirection != Vector3.zero)
                return;
            
            _pullDirection = character.transform.position - player.transform.position;
            _pullDirection.Normalize();
            
            _holdDirection = player.transform.position - character.transform.position;
            _holdDirection.Normalize();
        }
        
        private IEnumerator PullRoutine()
        {
            _isPull = true;
            animation.TrySetAnimationState(StateTag.PULL_STATE);
            player.PlayerRb.AddForce(_pullDirection * force, ForceMode2D.Impulse);
            yield return WhenDoneForce();
            
            _isPull = false;
            _isDonePull = true;
            animation.TrySetAnimationState(StateTag.IDLE_STATE);
        }
        
        private void HoldTarget()
        {
            _isHolding = true;
            character.LookAt(_holdDirection);
            animation.TrySetAnimationState(StateTag.HOLD_STATE);
            
            player.Flip();
            player.StopMovement();
            player.PlayerRb.AddForce(_holdDirection * holdForce, ForceMode2D.Impulse);
        }
    }
}