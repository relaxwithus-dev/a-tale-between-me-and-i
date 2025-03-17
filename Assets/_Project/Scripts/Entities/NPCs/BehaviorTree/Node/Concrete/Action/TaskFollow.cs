using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskFollow : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly float moveSpeed;
        private readonly float followTime;
        private readonly float followDelayTime = 1f;
        
        private readonly Vector3 rightDistance= new(2f, 0f, 0f);
        private readonly Vector3 leftDistance = new(-2f, 0f, 0f);

        private Transform _currentTarget;
        private Vector3 _targetPosition;
        private float _currentFollowTime;
        private float _currentFollowDelayTime;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsFollow = new()
        {
            { Emotion.Joy, (1, 0.2f, (3f, 8f)) },
            { Emotion.Trust, (1, 0.1f, (1f, 4.5f)) },
            { Emotion.Fear, (1, 0.6f, (1f, 3f)) },
            { Emotion.Surprise, (1, 0.2f, (3f, 8f)) },
            { Emotion.Sadness, (1, 0.5f, (1f, 4.5f)) },
            { Emotion.Disgust, (1, 0.5f, (1f, 4.5f)) },
            { Emotion.Anger, (1, 0.4f, (5f, 12f)) },
            { Emotion.Anticipation, (1, 0.5f, (1f, 4.5f)) }
        };


        // Constructor
        public TaskFollow(CharacterAI character, CharacterData data, float followTime)
        {
            this.character = character;
            this.followTime = followTime;
            
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
                return NodeStatus.Success;
            }
            
            if (_currentFollowDelayTime < followDelayTime)
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
            
            _targetPosition = GetDistancePosition(_currentTarget.position);
            return true;
        }
        
        private NodeStatus FollowTarget()
        {
            _currentFollowTime += Time.deltaTime;
            character.ChangeState(CharacterState.Walk);
            character.transform.position = Vector2.MoveTowards(character.transform.position, 
                _targetPosition, moveSpeed * Time.deltaTime);
            
            if (Vector3.Distance(character.transform.position, _targetPosition) <= 0.01f)
            {
                _currentFollowDelayTime = 0f;
                character.ChangeState(CharacterState.Idle);
            }
            
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