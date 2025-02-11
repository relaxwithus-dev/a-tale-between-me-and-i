using ATBMI.Core;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckTargetInZone : LeafWeight
    {
        private readonly float radius;
        private readonly Transform centerPoint;
        private readonly LayerMask targetLayer;
        private readonly float areaPriority;
        
        public CheckTargetInZone(Transform centerPoint, float radius, LayerMask targetLayer)
        {
            this.radius = radius;
            this.centerPoint = centerPoint;
            this.targetLayer = targetLayer;
            
            InitFactors(planning: 0f, risk: 0.3f, timeRange: (0, 0));
        }
        
        public override NodeStatus Evaluate()
        {
            Collider2D targetPhys = Physics2D.OverlapCircle(centerPoint.position, radius, targetLayer);
            if (targetPhys != null)
            {
                if (!targetPhys.CompareTag(GameTag.PLAYER_TAG))
                    return NodeStatus.Failure;
                
                Debug.Log("Execute: CheckTargetInZone");
                parentNode.SetData(TARGET_KEY, targetPhys.transform);
                parentNode.SetData(ORIGIN_KEY, centerPoint.position);
                return NodeStatus.Success;
            }
            
            return NodeStatus.Failure;
        }
    }
}