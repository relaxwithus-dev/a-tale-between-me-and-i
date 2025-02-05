using UnityEngine;
using ATBMI.Data;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToTarget : Leaf
    {
        private readonly CharacterAI character;
        private readonly CharacterData data;
        private readonly float moveDelay;
        private readonly bool isOrigin;
        private readonly bool isWalk;

        private CharacterState _targetState;
        private Vector3 _targetPosition;
        private float _currentTime;
        
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk, bool isOrigin, float moveDelay = 3f)
        {
            this.character = character;
            this.data = data;
            this.isWalk = isWalk;
            this.isOrigin = isOrigin;
            this.moveDelay = moveDelay;
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;
            
            if (_currentTime < moveDelay)
            {
                _currentTime += Time.deltaTime;
                ChangeDirectionToTarget();
                return NodeStatus.Running;
            }
            
            return MoveToTarget();
        }
        
        protected override void Reset()
        {
            base.Reset();
            _currentTime = 0f;
            _targetPosition = Vector3.zero;
            _targetState = CharacterState.None;
        }
        
        private bool TrySetupTarget()
        {
            if (_targetPosition != Vector3.zero)
                return true;
            
            if (isOrigin)
            {
                var originPoint = (Vector3)GetData(ORIGIN_KEY);
                if (originPoint == Vector3.zero)
                    return false;
            
                _targetPosition = originPoint;
            }
            else
            {
                var targetPoint = (Transform)GetData(TARGET_KEY);
                if (!targetPoint)
                    return false;
            
                _targetPosition = new Vector3(
                    targetPoint.position.x,
                    character.transform.position.y,
                    targetPoint.position.z);
            }

            return true;
        }
        
        private void ChangeDirectionToTarget()
        {
            if (_currentTime >= moveDelay / 2f)
            {
                var direction = (_targetPosition - character.transform.position).normalized;
                character.LookAt(direction);
            }
        }
        
        private NodeStatus MoveToTarget()
        {
            if (_targetState == CharacterState.None)
                SetupState();
                
            character.ChangeState(_targetState);
            character.transform.position = Vector2.MoveTowards(character.transform.position,
                _targetPosition, data.MoveSpeed * Time.deltaTime);

            if (!(Vector2.Distance(character.transform.position, _targetPosition) <= 0.01f)) 
                return NodeStatus.Running;
            
            character.transform.position = _targetPosition;
            character.ChangeState(CharacterState.Idle);
            
            _currentTime = 0f;
            _targetPosition = Vector3.zero;
            return NodeStatus.Success;
        }

        private void SetupState()
        {
            _targetState = isWalk 
                ? CharacterState.Walk 
                : CharacterState.Run;
        }
    }
}