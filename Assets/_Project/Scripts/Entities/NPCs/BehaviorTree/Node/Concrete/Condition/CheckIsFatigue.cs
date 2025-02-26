using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckIsFatigue : Leaf
    {
        private readonly CharacterManager manager;
        
        public CheckIsFatigue(CharacterManager manager)
        {
            this.manager = manager;
        }
        
        public override NodeStatus Evaluate()
        {
            if (!manager.IsEnergyEmpty())
            {
                Debug.Log("Execute Success: CheckIsFatigue");
                return NodeStatus.Success;
            }
                          
            Debug.LogWarning("Execute Failure: CheckIsFatigue");
            return NodeStatus.Failure;
        }
    }
}