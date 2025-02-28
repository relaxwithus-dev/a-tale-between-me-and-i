using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveBase : LeafWeight
    {
        protected readonly CharacterAI character;
        private readonly bool isWalk;
        private readonly float moveDelayTime = 3f;
        
        protected Vector3 targetPosition;
        private float _moveSpeed;
        private float _currentTime;
        private CharacterState _targetState;
        
        // Constructor
        protected TaskMoveBase(CharacterAI character, CharacterData data, bool isWalk)
        {
            this.character = character;
            this.isWalk = isWalk;
            
            _moveSpeed = data.GetSpeedByType(isWalk ? "Walk" : "Run");
            InitFactors(planning: 1f, risk: 0.4f, timeRange: (4, 8));
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
                Debug.Log("Execute Success: TaskMove");
                TrySetupStats();
            }
            
            character.ChangeState(_targetState);
            character.transform.position = Vector2.MoveTowards(character.transform.position,
                targetPosition, _moveSpeed * Time.deltaTime);

            if (!(Vector2.Distance(character.transform.position, targetPosition) <= 0.01f)) 
                return NodeStatus.Running;
            
            WhenReachTarget();
            ResetFields();
            return NodeStatus.Success;
        }
        
        private void ChangeDirectionToTarget()
        {
            if (!(_currentTime >= moveDelayTime / 2f))
                return;
            
            var direction = (targetPosition - character.transform.position).normalized;
            character.LookAt(direction);
        }
        
        protected virtual bool TrySetupTarget() { return false; }
        protected virtual void WhenReachTarget()
        {
            character.transform.position = targetPosition;
            character.ChangeState(CharacterState.Idle);
        }
        
        private void TrySetupStats()
        {
            _targetState = isWalk 
                ? CharacterState.Walk 
                : CharacterState.Run;
        }
        
        // Helpers
        private void ResetFields()
        {
            targetPosition = Vector3.zero;
            
            _moveSpeed = 0f;
            _currentTime = 0f;
            _targetState = CharacterState.Idle;
        }
    }
}