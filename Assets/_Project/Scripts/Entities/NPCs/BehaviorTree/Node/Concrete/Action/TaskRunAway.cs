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
        
        public TaskRunAway(CharacterAI character, CharacterData data, float moveTime)
        {
            this.character = character;
            this.data = data;
            this.moveTime = moveTime;
            
            InitFactors(planning: 1f, risk: 0.7f, timeRange: (3f, 8f));
        }
        
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
            character.ChangeState(CharacterState.Run);
            character.transform.Translate(_targetDirection * (data.MoveSpeed * Time.deltaTime));
            return NodeStatus.Running;
        }
        
        private bool TrySetupDirection()
        {
            if (_targetDirection != Vector3.zero)
                return true;
            
            var target = (Transform)GetData(TARGET_KEY);
            if (target == null)
                return false;
            
            _targetDirection = target.position - character.transform.position;
            _targetDirection.Normalize();
            character.LookAt(_targetDirection);
            
            // Opposite direction
            _targetDirection *= -1f;
            return true;
        }
    }
}