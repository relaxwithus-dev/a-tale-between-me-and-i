using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskMoveToOrigin : TaskMoveBase
    {
        public TaskMoveToOrigin(CharacterAI character, CharacterData data, bool isWalk) : base(character, data, isWalk)
        {
            InitFactors(plan: 1f, risk: 0.2f, timeRange: (3f, 6f));
        }
        
        protected override bool TrySetupTarget()
        {
            if (targetPosition != Vector3.zero)
                return true;

            var originPoint = (Vector3)GetData(ORIGIN_KEY);
            if (originPoint == Vector3.zero)
                return false;
            
            targetPosition = new Vector3(originPoint.x,
                character.transform.position.y,
                character.transform.position.z);

            return true;
        }
        
        protected override void WhenReachTarget()
        {
            base.WhenReachTarget();
            
            var direction = character.Direction * -1f;
            character.LookAt(direction);
            parentNode.ClearData(ORIGIN_KEY);
        }
    }
}