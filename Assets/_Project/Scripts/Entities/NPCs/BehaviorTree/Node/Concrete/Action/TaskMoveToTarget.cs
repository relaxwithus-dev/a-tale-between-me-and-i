using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToTarget : TaskMoveBase
    {
        private readonly Transform targetPoint;

        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk) : base(character, data, isWalk) { }
        public TaskMoveToTarget(CharacterAI character, CharacterData data, bool isWalk, Transform targetPoint) 
            : base(character, data, isWalk)
        {
            this.targetPoint = targetPoint;
        }
        
        protected override bool TrySetupTarget()
        {
            if (targetPosition != Vector3.zero)
                return true;

            if (targetPoint != null)
            {
                targetPosition = GetPositionFromTransform(targetPoint);
                return true;
            }
            
            var target = (Transform)GetData(TARGET_KEY);
            if (!target)
                return false;
            
            targetPosition = GetPositionFromTransform(target);
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
    }
}