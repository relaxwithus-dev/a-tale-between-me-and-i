using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveBase : LeafWeight
    {
        protected readonly CharacterAI character;
        private readonly CharacterData data;
        private readonly bool isWalk;
        private readonly float moveDelayTime = 1f;
        
        protected Vector3 targetPosition;
        private float _moveSpeed;
        private float _currentTime;
        private EntitiesState _targetState;
        
        // Constructor
        protected TaskMoveBase(CharacterAI character, CharacterData data, bool isWalk)
        {
            this.character = character;
            this.data = data;
            this.isWalk = isWalk;
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
            if (_targetState == EntitiesState.Idle)
                TrySetupStats();
            
            character.ChangeState(_targetState);
            character.transform.position = Vector2.MoveTowards(character.transform.position,
                targetPosition, _moveSpeed * Time.deltaTime);
            
            if (!(Vector2.Distance(character.transform.position, targetPosition) <= 0.01f)) 
                return NodeStatus.Running;
            
            Debug.Log("Execute Success: TaskMove");
            WhenReachTarget();
            ResetFields();
            return NodeStatus.Success;
        }
        
        private void ChangeDirectionToTarget()
        {
            if (!(_currentTime >= moveDelayTime / 2f))
                return;
            
            var direction = character.transform.position - targetPosition;
            direction.Normalize();
            character.LookAt(direction);
        }
        
        protected virtual bool TrySetupTarget() { return false; }
        protected virtual void WhenReachTarget()
        {
            character.transform.position = targetPosition;
            character.ChangeState(EntitiesState.Idle);
        }
        
        private void TrySetupStats()
        {
            _moveSpeed = data.GetSpeedByType(isWalk ? "Walk" : "Run");
            _targetState = isWalk 
                ? EntitiesState.Walk 
                : EntitiesState.Run;
        }
        
        // Helpers
        private void ResetFields()
        {
            targetPosition = Vector3.zero;
            
            _moveSpeed = 0f;
            _currentTime = 0f;
            _targetState = EntitiesState.Idle;
        }
    }
}