using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckTargetInProxemics : CheckTargetInArea
    {
        public CheckTargetInProxemics(Transform centerPoint, float radius, LayerMask targetLayer) 
            : base(centerPoint, radius, targetLayer) { }
        
        protected override void OnTargetEnter(Collider2D targetPhys)
        {
            base.OnTargetEnter(targetPhys);
            parentNode.SetData(PHYSIC_KEY, targetPhys);
            parentNode.SetData(TARGET_KEY, targetPhys.transform);
            parentNode.SetData(ORIGIN_KEY, centerPoint.position);
        }
    }
}