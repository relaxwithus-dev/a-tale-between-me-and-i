using UnityEngine;
using ATBMI.Core;

namespace ATBMI.Entities.NPCs
{
    public class CheckTargetInArea : LeafWeight
    {
        protected readonly Transform centerPoint;
        private readonly float radius;
        private readonly LayerMask targetLayer;
        
        public CheckTargetInArea(Transform centerPoint, float radius, LayerMask targetLayer)
        {
            this.centerPoint = centerPoint;
            this.radius = radius;
            this.targetLayer = targetLayer;
            
            InitFactors(plan: 0f, risk: 0.3f, timeRange: (0, 0));
        }
        
        public override NodeStatus Evaluate()
        {
            Collider2D targetPhys = Physics2D.OverlapCircle(centerPoint.position, radius, targetLayer);
            if (targetPhys != null)
            {
                if (!targetPhys.CompareTag(GameTag.ITEM_TAG))
                    return NodeStatus.Failure;
                
                Debug.Log("Execute Success: Check Target In Zone");
                OnTargetEnter(targetPhys);
                return NodeStatus.Success;
            }
            
            return NodeStatus.Failure;
        }
        
        protected virtual void OnTargetEnter(Collider2D targetPhys) { }
    }
}