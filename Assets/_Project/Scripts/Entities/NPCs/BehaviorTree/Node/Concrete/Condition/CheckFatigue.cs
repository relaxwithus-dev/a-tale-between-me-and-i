using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class CheckFatigue : Leaf
    {
        private readonly CharacterManager manager;
        
        public CheckFatigue(CharacterManager manager)
        {
            this.manager = manager;
        }
        
        public override NodeStatus Evaluate()
        {
            if (!manager.IsEnergyEmpty())
            {
                Debug.Log("Execute Success: CheckFatigue");
                return NodeStatus.Success;
            }
                          
            Debug.LogWarning("Execute Failure: CheckFatigue");
            return NodeStatus.Failure;
        }
    }
}