using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToTarget : TaskMoveBase
    {
        private Transform _targetPoint;
        private bool _isInitialPoint = true;

        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk) : base(character, data, isWalk) { }
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk, Transform targetPoint) 
            : base(character, data, isWalk)
        {
            _targetPoint = targetPoint;
        }
        
        protected override bool TrySetupTarget()
        {
            if (!_isInitialPoint)
                return true;
            
            if (!_targetPoint)
            {
                _targetPoint = (Transform)GetData(TARGET_KEY);
                if (_targetPoint)
                    _isInitialPoint = false;
                else
                    return false;
            }
            
            targetPosition = GetPositionFromTransform(_targetPoint);
            return true;
        }
        
        private Vector3 GetPositionFromTransform(Transform transform)
        {
            return new Vector3(
                transform.position.x,
                character.transform.position.y,
                character.transform.position.z
            );
        }
        
        protected override void WhenReachTarget()
        {
            base.WhenReachTarget();
            parentNode.ClearData(TARGET_KEY);
        }

        protected override void Reset()
        {
            base.Reset();
            _isInitialPoint = true;
        }
    }
}