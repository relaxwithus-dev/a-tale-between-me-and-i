using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToTarget : TaskMoveBase
    {
        private readonly Transform initialPoint;
        private readonly Vector3 rightDistance = new(1f, 0f, 0f);
        private readonly Vector3 leftDistance = new(-1f, 0f, 0f);

        private Transform _targetPoint;

        // Constructor
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk) : base(character, data, isWalk) { }
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk, Transform initialPoint) 
            : base(character, data, isWalk)
        {
            this.initialPoint = initialPoint;
            InitFactors(plan: 1f, risk: 0.3f, timeRange: (4, 7));
        }
        
        // Core
        protected override bool TrySetupTarget()
        {
            if (initialPoint != null)
            {
                if (targetPosition == Vector3.zero)
                    targetPosition = GetPosition(initialPoint);
                
                return true;
            }
            
            _targetPoint = (Transform)GetData(TARGET_KEY);
            if (!_targetPoint)
                return false;
            
            targetPosition = GetPositionWithDistance(_targetPoint.position);
            return true;
        }
        
        private Vector3 GetPosition(Transform target)
        {
            return new Vector3(target.position.x,
                character.transform.position.y,
                character.transform.position.z
            );
        }
        
        private Vector3 GetPositionWithDistance(Vector3 target)
        {
            var opposite = target.x < character.transform.position.x ? rightDistance : leftDistance;
            return new Vector3((target + opposite).x,
                character.transform.position.y,
                character.transform.position.z);
        }
        
        protected override void WhenReachTarget()
        {
            base.WhenReachTarget();
            parentNode.ClearData(TARGET_KEY);
        }
    }
}