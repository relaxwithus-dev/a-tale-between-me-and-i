using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskObserve : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterAnimation animation;
        private readonly float offRange;
        
        private Transform _currentTarget;
        private Vector3 _targetPosition;
        
        private const float OFFSET = 0.5f;
        private const string OBSERVE_STATE = "Observe";
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsObserve = new()
        {
            { Emotion.Joy, (1, 0.5f, (2f, 4f)) },
            { Emotion.Trust, (1, 0.5f, (3f, 7f)) },
            { Emotion.Fear, (1, 0.4f, (2f, 4f)) },
            { Emotion.Surprise, (1, 0.5f, (2f, 4f)) },
            { Emotion.Sadness, (1, 0.4f, (2f, 3f)) },
            { Emotion.Disgust, (1, 0.4f, (2f, 3f)) },
            { Emotion.Anger, (1, 0.4f, (3f, 7f)) },
            { Emotion.Anticipation, (1, 0.2f, (0.5f, 3.5f)) }
        };

        
        // Constructor
        public TaskObserve(CharacterAI character, CharacterAnimation animation, float offRange)
        {
            this.character = character;
            this.animation = animation;
            this.offRange = offRange;
            
            OverrideEmotionFactors(_factorsObserve);
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;

            if (CheckOffRange())
            {
                Debug.Log("Execute Success: TaskObserve");
                return NodeStatus.Success;
            }
            
            _targetPosition = character.transform.position - _currentTarget.transform.position;
            _targetPosition.Normalize();
            character.LookAt(_targetPosition);
            animation.TrySetAnimationState(OBSERVE_STATE);
            
            return NodeStatus.Running;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _currentTarget = null;
            _targetPosition = Vector3.zero;
        }
        
        private bool TrySetupTarget()
        {
            if (_currentTarget != null)
                return true;
            
            var target = (Transform)GetData(TARGET_KEY);
            if (target == null)
            {
                Debug.LogWarning("Execute Failure: TaskObserve");
                return false;
            }
            
            _currentTarget = target;
            return true;
        }
        
        private bool CheckOffRange()
        {
            var characterX = character.transform.position.x;
            var targetX = _currentTarget.transform.position.x;
            var bound = offRange + OFFSET;
            
            return targetX < characterX - bound || targetX > characterX + bound;
        }
    }
}