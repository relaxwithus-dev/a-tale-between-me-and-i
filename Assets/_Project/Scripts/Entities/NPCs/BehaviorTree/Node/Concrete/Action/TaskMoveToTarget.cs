using UnityEngine;
using ATBMI.Data;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToTarget : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterData data;
        private readonly bool isWalk;
        private readonly float moveDelayTime = 3f;
        
        private CharacterState _targetState;
        private Vector3 _targetPosition;
        private float _currentTime;
        
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk, Transform targetPoint)
        {
            this.character = character;
            this.data = data;
            this.isWalk = isWalk;
            
            InitFactors(planning: 1f, risk: 0.4f, timeRange: (5, 10));
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
            {
                Debug.Log("Execute Failure: TaskMoveToTarget");
                return NodeStatus.Failure;
            }
            
            if (_currentTime < moveDelayTime)
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
            ResetFields();
        }
        
        private NodeStatus MoveToTarget()
        {
            if (_targetState == CharacterState.Idle)
            {
                Debug.Log("Execute Success: TaskMoveToTarget");
                SetupState();
            }
                
            character.ChangeState(_targetState);
            character.transform.position = Vector2.MoveTowards(character.transform.position,
                _targetPosition, data.MoveSpeed * Time.deltaTime);

            if (!(Vector2.Distance(character.transform.position, _targetPosition) <= 0.01f)) 
                return NodeStatus.Running;
            
            character.transform.position = _targetPosition;
            character.ChangeState(CharacterState.Idle);
            
            ResetFields();
            return NodeStatus.Success;
        }
        
        private void ChangeDirectionToTarget()
        {
            if (!(_currentTime >= moveDelayTime / 2f)) return;
            var direction = (_targetPosition - character.transform.position).normalized;
            character.LookAt(direction);
        }

        // Helpers
        private bool TrySetupTarget()
        {
            if (_targetPosition != Vector3.zero)
                return true;
            
            var targetPoint = (Transform)GetData(TARGET_KEY);
            if (targetPoint == null)
                return false;
            
            _targetPosition = new Vector3(targetPoint.position.x,
                character.transform.position.y, 
                character.transform.position.z);
            
            return true;
        }
        
        private void SetupState()
        {
            _targetState = isWalk 
                ? CharacterState.Walk 
                : CharacterState.Run;
        }
        
        private void ResetFields()
        {
            _currentTime = 0f;
            _targetPosition = Vector3.zero;
            _targetState = CharacterState.Idle;
        }
    }
}