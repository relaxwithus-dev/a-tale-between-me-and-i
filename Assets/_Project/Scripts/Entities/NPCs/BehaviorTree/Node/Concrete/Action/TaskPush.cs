using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskPush : TaskForceBase
    {
        // Constructor
        public TaskPush(CharacterAI character, float force, float delay) : base(character, force, delay)
        {
            InitFactors(plan: 1, risk: 0.7f, timeRange: (0.3f, 1f));
        }
        
        // Core
        protected override NodeStatus PerformForce()
        {
            Vector2 direction = (player.transform.position - character.transform.position).normalized;
            
            player.StopMovement();
            player.PlayerRb.AddForce(direction * force, ForceMode2D.Impulse);
            player.StartCoroutine(WhenDoneForce());

            return NodeStatus.Success;
        }
    }
}