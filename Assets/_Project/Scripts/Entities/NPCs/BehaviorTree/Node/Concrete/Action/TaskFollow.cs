using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskFollow : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterData data;
        private readonly float followTime;
        private readonly float followDelayTime = 1f;
        
        private readonly Vector3 rightDistance = new(2f, 0f, 0f);
        private readonly Vector3 leftDistance = new(-2f, 0f, 0f);

        private Transform _currentTarget;
        private Vector3 _targetPosition;
        private float _currentFollowTime;
        private float _currentFollowDelayTime;

        // Constructor
        public TaskFollow(CharacterAI character, CharacterData data, float followTime)
        {
            this.character = character;
            this.data = data;
            this.followTime = followTime;
            
            InitFactors(planning: 1f, risk: 0.5f, timeRange: (6f, 12f));
        }

        // Core
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;

            if (_currentFollowTime > followTime)
                return NodeStatus.Success;

            if (_currentFollowDelayTime < followDelayTime)
            {
                _currentFollowDelayTime += Time.deltaTime;
                return NodeStatus.Running;
            }
            
            return FollowTarget();
        }
        
        private bool TrySetupTarget()
        {
            if (!_currentTarget)
            {
                Debug.Log("Execute Success: TaskFollow");
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
                _targetPosition, data.MoveSpeed * Time.deltaTime);
            
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