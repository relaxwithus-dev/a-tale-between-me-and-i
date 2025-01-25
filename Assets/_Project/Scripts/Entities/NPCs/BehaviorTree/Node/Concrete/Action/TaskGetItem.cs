using ATBMI.Item;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskGetItem : Node
    {
        public override NodeStatus Evaluate()
        {
            var target = (Transform)GetData("Target");
            if (!target)
                return NodeStatus.Failure;

            if (target.TryGetComponent<ItemController>(out var item))
            {
                Debug.Log($"get fruit {item.name}");
                Object.Destroy(item.gameObject);
                return NodeStatus.Success;
            }
            
            return NodeStatus.Failure;
        }
    }
}