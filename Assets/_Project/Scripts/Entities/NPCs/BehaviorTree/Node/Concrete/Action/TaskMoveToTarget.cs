using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToTarget : TaskMoveBase
    {
        private readonly Transform initialPoint;
        private readonly Vector3 rightDistance = new(1f, 0f, 0f);
        private readonly Vector3 leftDistance = new(-1f, 0f, 0f);

        private Transform _targetPoint;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsMoveToTarget = new()
        {
            { Emotion.Joy, (1, 0.3f, (3f, 7f)) },
            { Emotion.Trust, (1, 0.3f, (3f, 7f)) },
            { Emotion.Fear, (1, 0.5f, (3f, 7f)) },
            { Emotion.Surprise, (1, 0.4f, (3f, 7f)) },
            { Emotion.Sadness, (1, 0.4f, (2f, 4f)) },
            { Emotion.Disgust, (1, 0.4f, (2f, 4f)) },
            { Emotion.Anger, (1, 0.2f, (0.15f, 0.2f)) },
            { Emotion.Anticipation, (1, 0.4f, (2f, 4f)) }
        };


        // Constructor
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk) : base(character, data, isWalk) { }
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk, Transform initialPoint) 
            : base(character, data, isWalk)
        {
            this.initialPoint = initialPoint;
            OverrideEmotionFactors(_factorsMoveToTarget);
        }
        
        // Core
        protected override bool TrySetupTarget()
        {
            if (initialPoint != null)
            {
                if (targetPosition == Vector3.zero)
                    targetPosition = GetPosition(initialPoint);
                
                return true;
            }
            
            _targetPoint = (Transform)GetData(TARGET_KEY);
            if (!_targetPoint)
                return false;
            
            targetPosition = GetPositionWithDistance(_targetPoint.position);
            return true;
        }
        
        private Vector3 GetPosition(Transform target)
        {
            return new Vector3(target.position.x,
                character.transform.position.y,
                character.transform.position.z
            );
        }
        
        private Vector3 GetPositionWithDistance(Vector3 target)
        {
            var opposite = target.x < character.transform.position.x ? rightDistance : leftDistance;
            return new Vector3((target + opposite).x,
                character.transform.position.y,
                character.transform.position.z);
        }
        
        protected override void WhenReachTarget()
        {
            base.WhenReachTarget();
            parentNode.ClearData(TARGET_KEY);
        }
    }
}