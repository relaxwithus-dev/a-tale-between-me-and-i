using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskPull : LeafWeight
    {
        public TaskPull()
        {
            InitFactors(planning: 1, risk: 0.8f, timeRange: (0.5f, 1.5f));
        }
        
        public override NodeStatus Evaluate()
        {
            Debug.Log("Execute: TaskPull");
            return base.Evaluate();
        }
    }
}