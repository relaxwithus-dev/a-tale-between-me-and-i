using UnityEngine;
using ATBMI.Core;

namespace ATBMI.Entities.NPCs
{
    public class CheckTargetInArea : LeafWeight
    {
        protected readonly Transform centerPoint;
        private readonly float radius;
        private readonly LayerMask targetLayer;

        private Collider2D _latestTarget;
        
        public CheckTargetInArea(Transform centerPoint, float radius, LayerMask targetLayer)
        {
            this.centerPoint = centerPoint;
            this.radius = radius;
            this.targetLayer = targetLayer;
        }
        
        public override NodeStatus Evaluate()
        {
            if (!CheckTargetLeaving())
                return NodeStatus.Failure;
            
            Collider2D targetPhys = Physics2D.OverlapCircle(centerPoint.position, radius, targetLayer);
            if (targetPhys != null)
            {
                if (!targetPhys.CompareTag(GameTag.PLAYER_TAG))
                    return NodeStatus.Failure;
                
                Debug.LogWarning("Execute Success: CheckTargetInArea");
                OnTargetEnter(targetPhys);
                _latestTarget = targetPhys;
                return NodeStatus.Success;
            }
            
            return NodeStatus.Failure;
        }
        
        private bool CheckTargetLeaving()
        {
            // Player is leaving
            if (_latestTarget == null)
                return true;
            
            // Player still on area
            if (Physics2D.OverlapCircle(centerPoint.position, radius, targetLayer))
                return false;
            
            parentNode.ClearDataContext();
            _latestTarget = null;
            return true;
        }
        
        protected virtual void OnTargetEnter(Collider2D targetPhys) { }
    }
}