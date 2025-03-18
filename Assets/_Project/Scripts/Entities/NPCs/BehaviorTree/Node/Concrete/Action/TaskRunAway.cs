using System.Collections.Generic;
using ATBMI.Data;
using ATBMI.Interaction;
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
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsRunAway = new()
        {
            { Emotion.Joy, (1, 0.7f, (3f, 6f)) },
            { Emotion.Trust, (1, 0.7f, (4f, 9f)) },
            { Emotion.Fear, (1, 0.2f, (2f, 4f)) },
            { Emotion.Surprise, (1, 0.7f, (3f, 6f)) },
            { Emotion.Sadness, (1, 0.6f, (4f, 9f)) },
            { Emotion.Disgust, (1, 0.6f, (4f, 9f)) },
            { Emotion.Anger, (1, 0.3f, (4f, 9f)) },
            { Emotion.Anticipation, (1, 0.6f, (4f, 9f)) }
        };
        
        // Constructor
        public TaskRunAway(CharacterAI character, CharacterData data, float moveTime)
        {
            this.character = character;
            this.moveTime = moveTime;
            
            moveSpeed = data.GetSpeedByType("Run");
            OverrideEmotionFactors(_factorsRunAway);
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
                Debug.Log("Execute Success: TaskRunAway");
                
                _targetDirection *= -1f;
                character.LookAt(_targetDirection);
                character.ChangeState(CharacterState.Idle);
                
                parentNode.ClearData(TARGET_KEY);
                InteractEvent.RestrictedEvent(false);
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
            InteractEvent.RestrictedEvent(true);
            return true;
        }
    }
}