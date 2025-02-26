using System.Collections;
using ATBMI.Entities.Player;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskForceBase : LeafWeight
    {
        protected readonly CharacterAI character;
        protected readonly float force;
        private readonly float delay;
        
        protected PlayerController player;
        
        protected TaskForceBase(CharacterAI character, float force, float delay)
        {
            this.character = character;
            this.force = force;
            this.delay = delay;
        }

        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;
            
            return PerformForce();
        }
        
        protected virtual NodeStatus PerformForce() { return NodeStatus.Failure; }
        
        private bool TrySetupTarget()
        {
            if (player != null)
                return true;
            
            var target = (Collider2D)GetData(PHYSIC_KEY);
            if (!target)
            {
                Debug.LogWarning("Execute Failure: TaskForce");
                return false;
            }
            
            Debug.Log("Execute Success: TaskForce");
            player = target.GetComponent<PlayerController>();
            return true;
        }
        
        protected IEnumerator WhenDoneForce()
        {
            yield return new WaitForSeconds(delay);
            player.PlayerRb.velocity = Vector2.zero;
            player.StartMovement();
            
            parentNode.ClearData(PHYSIC_KEY);
        }
    }
}