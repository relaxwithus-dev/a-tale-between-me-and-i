using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskRunAway : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly CharacterData data;
        private readonly float moveTime;
        private readonly float delayTime = 2f;
                
        private float _currentDelayTime;
        private float _currentMoveTime;
        private Vector3 _targetDirection;
        
        // Constructor
        public TaskRunAway(CharacterAI character, CharacterData data, float moveTime)
        {
            this.character = character;
            this.data = data;
            this.moveTime = moveTime;
            
            InitFactors(planning: 1f, risk: 0.7f, timeRange: (3f, 8f));
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
        
        // TODO: Ganti state ke-Run
        private NodeStatus RunAway()
        {
            if (_currentMoveTime >= moveTime)
            {
                _targetDirection *= -1f;
                character.LookAt(_targetDirection);
                character.ChangeState(CharacterState.Idle);
                
                parentNode.ClearData(TARGET_KEY);
                return NodeStatus.Success;
            }
            
            _currentMoveTime += Time.deltaTime;
            character.LookAt(_targetDirection);
            character.ChangeState(CharacterState.Walk);
            character.transform.Translate(Vector3.right * (data.MoveSpeed * Time.deltaTime));
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
            Debug.Log("Execute Success: TaskRunAway");
            return true;
        }
    }
}