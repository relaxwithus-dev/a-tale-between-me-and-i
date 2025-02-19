using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckTargetAvailable : Leaf
    {
        private Transform _targetPoint;

        public CheckTargetAvailable(Transform targetPoint)
        {
            _targetPoint = targetPoint;
        }
        
        public override NodeStatus Evaluate()
        {
            if (_targetPoint)
            {
                Debug.Log("Execute Success: CheckTargetAvailable");
                parentNode.SetData(TARGET_KEY, _targetPoint);
                _targetPoint = null;
                return NodeStatus.Success;
            }
            
            Debug.LogWarning("Execute Failure: CheckTargetAvailable");
            return NodeStatus.Failure;
        }
    }
}