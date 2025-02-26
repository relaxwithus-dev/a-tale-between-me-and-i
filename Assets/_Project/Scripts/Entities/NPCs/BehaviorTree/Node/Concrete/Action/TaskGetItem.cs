using UnityEngine;
using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class TaskGetItem : Leaf
    {
        public override NodeStatus Evaluate()
        {
            var target = (Transform)GetData("Target");
            if (!target)
                return NodeStatus.Failure;

            if (target.TryGetComponent<ItemInteract>(out var item))
            {
                Debug.Log($"Execute: Get Item {item.name}");
                Object.Destroy(item.gameObject);
                return NodeStatus.Success;
            }
            
            return NodeStatus.Failure;
        }
    }
}