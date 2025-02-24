using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskPatrol : Leaf
    {
        // Fields
        private readonly CharacterAI character;
        private readonly CharacterData data;
        private readonly CharacterManager manager;
        
        private readonly List<Vector3> wayPoints = new();
        private readonly float moveDelayTime = 3f;
        private readonly int minWayPoints = 2;
        
        private int _currentIndex;
        private float _currentTime;
        private bool _isPathCalculated = true;
        private bool _isValidated;
        private Vector3 _currentTarget;
        
        // Constructor
        public TaskPatrol(CharacterAI character, CharacterManager manager, CharacterData data, Transform[] wayPoints)
        {
            this.character = character;
            this.manager = manager;
            this.data = data;
            foreach (var point in wayPoints)
            {
                var pointPosition = point.position;
                this.wayPoints.Add(pointPosition);
            }
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            if (!Validate() && !_isValidated)
                return NodeStatus.Failure;

            if (_currentIndex >= wayPoints.Count)
                return NodeStatus.Success;
            
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
            manager.DecreaseEnergy();
            character.ChangeState(CharacterState.Walk);
            character.transform.position = Vector2.MoveTowards(character.transform.position,
                target, data.MoveSpeed * Time.deltaTime);
            
            // Set path-way
            if (Vector3.Distance(character.transform.position, target) <= 0.01f)
            {
                _currentIndex++;
                _currentTime = 0f;
                _isPathCalculated = true;
                character.ChangeState(CharacterState.Idle);
            }
            
            return NodeStatus.Running;
        }
        
        private void ChangeDirectionToTarget(Vector3 target)
        {
            if (!(_currentTime >= moveDelayTime / 2f)) return;
            var direction = target - character.transform.position;
            direction.Normalize();
            character.LookAt(direction);
        }
        
        // Helpers
        private bool Validate()
        {
            if (wayPoints.Count < minWayPoints)
            {
                Debug.LogWarning("Execute Failure: TaskPatrol");
                _isValidated = true;
                return false;
            }
            
            Debug.Log("Execute Success: TaskPatrol");
            _isValidated = true;
            return true;
        }
    }
}