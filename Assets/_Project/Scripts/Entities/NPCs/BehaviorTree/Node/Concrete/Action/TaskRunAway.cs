using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskRunAway : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly float moveSpeed;
        private readonly float moveTime;
        private readonly float delayTime = 2f;
                
        private float _currentDelayTime;
        private float _currentMoveTime;
        private Vector3 _targetDirection;
        
        // Constructor
        public TaskRunAway(CharacterAI character, CharacterData data, float moveTime)
        {
            this.character = character;
            this.moveTime = moveTime;
            
            moveSpeed = data.GetSpeedByType("Run");
            InitFactors(plan: 1f, risk: 0.7f, timeRange: (3f, 8f));
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            if (!TrySetupDirection())
                return NodeStatus.Failure;
            
            if (_currentDelayTime < delayTime)
            { 
                _currentDelayTime += Time.deltaTime;
                return NodeStatus.Running;
            }
            
            return RunAway();
        }
        
        protected override void Reset()
        {
            base.Reset();
            _currentMoveTime = 0f;
            _currentDelayTime = 0f;
            _targetDirection = Vector3.zero;
        }
        
        private NodeStatus RunAway()
        {
            if (_currentMoveTime >= moveTime)
            {
                _targetDirection *= -1f;
                character.LookAt(_targetDirection);
                character.ChangeState(CharacterState.Idle);
                
                Debug.Log("Execute Success: TaskRunAway");
                parentNode.ClearData(TARGET_KEY);
                return NodeStatus.Success;
            }
            
            _currentMoveTime += Time.deltaTime;
            character.LookAt(_targetDirection);
            character.ChangeState(CharacterState.Run);
            character.transform.Translate(Vector3.right * (moveSpeed * Time.deltaTime));
            return NodeStatus.Running;
        }
        
        private bool TrySetupDirection()
        {
            if (_targetDirection != Vector3.zero)
                return true;
            
            var target = (Transform)GetData(TARGET_KEY);
            if (target == null)
            { 
                Debug.LogWarning("Execute Failure: TaskRunAway");
                return false;
            }
            
            _targetDirection = target.position - character.transform.position;
            _targetDirection.Normalize();
            character.LookAt(_targetDirection);
            
            // Opposite direction
            _targetDirection *= -1f;
            return true;
        }
    }
}