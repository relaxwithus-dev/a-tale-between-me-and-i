using UnityEngine;
using ATBMI.Data;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToTarget : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterData data;
        private readonly bool isWalk;
        private readonly bool isAway;
        private readonly Transform targetPoint;
        private readonly float moveDelayTime = 3f;
        
        private CharacterState _targetState;
        private Vector3 _targetPosition;
        private float _currentTime;
        
        // Constructor
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk, Transform targetPoint)
        {
            this.character = character;
            this.data = data;
            this.isWalk = isWalk;
            this.targetPoint = targetPoint;
            
            InitFactors(planning: 1f, risk: 0.4f, timeRange: (5, 10));
        }
        
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk, bool isAway)
        {
            this.character = character;
            this.data = data;
            this.isWalk = isWalk;
            this.isAway = isAway;
            
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
                TrySetupState();
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

        private void TrySetupState()
        {
            _targetState = isWalk 
                ? CharacterState.Walk 
                : CharacterState.Run;
        }
        
        private bool TrySetupTarget()
        {
            if (_targetPosition != Vector3.zero)
                return true;

            if (targetPoint != null)
            {
                _targetPosition = GetPositionFromTransform(targetPoint);
                return true;
            }
            
            // Get origin/target pos
            var dataKey = isAway ? TARGET_KEY : ORIGIN_KEY;
            var positionData = GetData(dataKey);

            if (positionData is Transform transform)
                _targetPosition = GetPositionFromTransform(transform);
            else if (positionData is Vector3 vector && vector != Vector3.zero)
                _targetPosition = ModifyPosition(vector.x);
            else
                return false;
            
            return true;
        }

        // Helpers
        private void ResetFields()
        {
            _currentTime = 0f;
            _targetPosition = Vector3.zero;
            _targetState = CharacterState.Idle;
        }
        
        private Vector3 GetPositionFromTransform(Transform transform)
        {
            return new Vector3(
                transform.position.x,
                character.transform.position.y,
                character.transform.position.z
            );
        }

        private Vector3 ModifyPosition(float xValue)
        {
            return new Vector3(
                xValue,
                character.transform.position.y,
                character.transform.position.z
            );
        }
    }
}