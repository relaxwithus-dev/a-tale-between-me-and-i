using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckTargetInZone : Node
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
        }
        
        public override NodeStatus Evaluate()
        {
            Collider2D targetPhys = Physics2D.OverlapCircle(centerPoint.position, radius, targetLayer);
            if (targetPhys != null)
            {
                if (!targetPhys.CompareTag("Player")) 
                    return NodeStatus.Failure;
                
                parentNode.SetData("Target", targetPhys.transform);
                parentNode.SetData("Origin", centerPoint.position);
                return NodeStatus.Success;
            }
            
            return NodeStatus.Failure;
        }
    }
}