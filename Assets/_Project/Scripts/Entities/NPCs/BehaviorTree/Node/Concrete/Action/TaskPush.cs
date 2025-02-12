using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskPush : LeafWeight
    {
        public TaskPush()
        {
            InitFactors(planning: 1, risk: 0.7f, timeRange: (0.3f, 1f));
        }
        
        public override NodeStatus Evaluate()
        {
            Debug.Log("Execute: TaskPush");
            return base.Evaluate();
        }
    }
}