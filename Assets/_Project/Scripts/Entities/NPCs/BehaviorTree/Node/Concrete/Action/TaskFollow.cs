using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;

namespace ATBMI.Entities.NPCs
{
    public class TaskFollow : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly float moveSpeed;
        private readonly float followTime;
        private readonly float delayTime;
        
        private readonly Vector3 rightDistance= new(2f, 0f, 0f);
        private readonly Vector3 leftDistance = new(-2f, 0f, 0f);

        private Transform _currentTarget;
        private Vector3 _targetPosition;
        private Vector3 _targetDirection;
        
        private float _currentFollowTime;
        private float _currentFollowDelayTime;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsFollow = new()
        {
            { Emotion.Joy, (1, 0.2f, (3f, 8f)) },
            { Emotion.Trust, (0.5f, 0.4f, (1f, 3.5f)) },
            { Emotion.Fear, (1, 0.6f, (1f, 3f)) },
            { Emotion.Surprise, (1, 0.2f, (3f, 8f)) },
            { Emotion.Sadness, (1, 0.5f, (1f, 4.5f)) },
            { Emotion.Disgust, (1, 0.5f, (1f, 4.5f)) },
            { Emotion.Anger, (1, 0.4f, (5f, 12f)) },
            { Emotion.Anticipation, (1, 0.5f, (1f, 4.5f)) }
        };
        
        // Constructor
        public TaskFollow(CharacterAI character, CharacterData data, float followTime, float delayTime = 1f)
        {
            this.character = character;
            this.followTime = followTime;
            this.delayTime = delayTime;
            
            moveSpeed = data.GetSpeedByType("Walk");
            OverrideEmotionFactors(_factorsFollow);
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;
            
            if (_currentFollowTime > followTime)
            {
                Debug.Log("Execute Success: TaskFollow");
                parentNode.ClearData(TARGET_KEY);
                character.ChangeState(CharacterState.Idle);
                return NodeStatus.Success;
            }
            
            if (_currentFollowDelayTime < delayTime)
            {
                _currentFollowDelayTime += Time.deltaTime;
                return NodeStatus.Running;
            }
            
            return FollowTarget();
        }
        
        protected override void Reset()
        {
            base.Reset();
            
            _currentTarget = null;
            _currentFollowTime = 0f;
            _currentFollowDelayTime = 0f;
            _targetPosition = Vector3.zero;
            _targetDirection = Vector3.zero;
        }
        
        private bool TrySetupTarget()
        {
            if (!_currentTarget)
            {
                _currentTarget = (Transform)GetData(TARGET_KEY);
                if (!_currentTarget)
                {
                    Debug.LogWarning("Execute Failure: TaskFollow");
                    return false;
                }
            }
            
            // Setup direction
            var targetPos = _currentTarget.position;
            var characterPos = character.transform.position;
            
            _targetDirection = targetPos - character.transform.position;
            _targetDirection.Normalize();
            
            // Setup position
            var distancePos = GetDistancePosition(targetPos);
            var isTargetInRange = (distancePos.x < targetPos.x && characterPos.x >= distancePos.x + 0.5f) ||
                                   (distancePos.x > targetPos.x && characterPos.x <= distancePos.x - 0.5f);
            
            if (!isTargetInRange)
            {
                _targetPosition = distancePos;
            }
            return true;
        }
        
        private NodeStatus FollowTarget()
        {
            if (Vector3.Distance(character.transform.position, _targetPosition) <= 0.01f)
            {
                _currentFollowDelayTime = 0f;
                character.ChangeState(CharacterState.Idle);
                return NodeStatus.Running;
            }
            
            _currentFollowTime += Time.deltaTime;
            character.LookAt(_targetDirection);
            character.ChangeState(CharacterState.Walk);
            character.transform.position = Vector2.MoveTowards(character.transform.position, 
                _targetPosition, moveSpeed * Time.deltaTime);
            
            return NodeStatus.Running;
        }
        
        private Vector3 GetDistancePosition(Vector3 target)
        {
            var opposite = target.x < character.transform.position.x ? rightDistance : leftDistance;
            return new Vector3((target + opposite).x,
                character.transform.position.y,
                character.transform.position.z);
        }
    }
}