using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;

namespace ATBMI.Entities.NPCs
{
    public class TaskPatrol : Leaf
    {
        // Fields
        private readonly CharacterAI character;
        private readonly float moveSpeed;
        private readonly CharacterState targetState = CharacterState.Walk;
        private readonly CheckFatigue fatigue;
        
        private readonly List<Vector3> wayPoints = new();
        private readonly float moveDelayTime;
        private const int MinWayPoints = 2;
        
        private int _currentIndex;
        private float _currentTime;
        private bool _isPathCalculated = true;
        private bool _isValidated;
        private Vector3 _currentTarget;
        
        // Constructor
        public TaskPatrol(CharacterAI character, CheckFatigue fatigue, Transform[] wayPoints, float moveDelayTime = 2f)
        {
            this.character = character;
            this.fatigue = fatigue;
            this.moveDelayTime = moveDelayTime;
            
            moveSpeed = character.Data.GetSpeedByType(targetState.ToString());
            foreach (var point in wayPoints)
            {
                var pointPosition = new Vector3(point.localPosition.x, character.transform.localPosition.y, point.localPosition.z);
                Debug.Log(pointPosition);
                this.wayPoints.Add(pointPosition);
            }
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            if (!Validate() && !_isValidated)
                return NodeStatus.Failure;

            if (_currentIndex >= wayPoints.Count)
            {
                Debug.Log("Execute Success: TaskPatrol"); 
                return NodeStatus.Success;
            }
            
            _currentTarget = wayPoints[_currentIndex];
            if (_currentTime < moveDelayTime && _isPathCalculated)
            {
                _currentTime += Time.deltaTime;
                ChangeDirectionToTarget(_currentTarget);
                return NodeStatus.Running;
            }
            
            _isPathCalculated = false;
            return Patrol(_currentTarget);
        }
        
        protected override void Reset()
        {
            base.Reset();
            _currentIndex = 0;
            _currentTime = 0f;
            _isPathCalculated = true;
            _currentTarget = wayPoints[_currentIndex];
        }
        
        private NodeStatus Patrol(Vector3 target)
        {
            fatigue.ModifyStamina();
            character.ChangeState(targetState);
            character.transform.localPosition = Vector2.MoveTowards(character.transform.localPosition,
                target, moveSpeed * Time.deltaTime);
            
            // Set path-way
            if (Vector3.Distance(character.transform.localPosition, target) <= 0.01f)
            {
                _currentIndex++;
                _currentTime = 0f;
                _isPathCalculated = true;
                Debug.Log("reach way points");
                character.ChangeState(CharacterState.Idle);
            }
            
            return NodeStatus.Running;
        }
        
        private void ChangeDirectionToTarget(Vector3 target)
        {
            if (!(_currentTime >= moveDelayTime / 2f)) return;
            var direction = target - character.transform.localPosition;
            direction.Normalize();
            character.LookAt(direction);
        }
        
        // Helpers
        private bool Validate()
        {
            if (wayPoints.Count < MinWayPoints)
            {
                Debug.LogWarning("Execute Failure: TaskPatrol");
                _isValidated = false;
                return false;
            }
            
            _isValidated = true;
            return true;
        }
    }
}