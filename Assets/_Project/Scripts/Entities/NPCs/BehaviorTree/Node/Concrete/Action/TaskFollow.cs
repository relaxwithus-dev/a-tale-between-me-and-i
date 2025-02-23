using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskFollow : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterData data;
        private readonly float followTime;
        private readonly float followDelayTime = 0.5f;
        private readonly Vector3 followDistance = new(0f, 3f, 0f);
        
        private Vector3 _targetPosition;
        private float _currentFollowTime;
        private float _currentFollowDelayTime;

        public TaskFollow(CharacterAI character, CharacterData data, float followTime)
        {
            this.character = character;
            this.data = data;
            this.followTime = followTime;
        }

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
            if (_targetPosition != Vector3.zero)
                return true;
            
            var target = (Transform)GetData(TARGET_KEY);
            if (!target)
                return false;
            
            _targetPosition = target.position + followDistance;
            return true;
        }
        
        private NodeStatus FollowTarget()
        {
            _currentFollowTime += Time.deltaTime;
            character.transform.position = Vector2.MoveTowards(character.transform.position, 
                _targetPosition, data.MoveSpeed * Time.deltaTime);
            
            if (Vector3.Distance(character.transform.position, _targetPosition) <= 0.01f)
            {
                _currentFollowDelayTime = 0f;
                character.ChangeState(CharacterState.Idle);
            }
            
            return NodeStatus.Running;
        }
        
    }
}