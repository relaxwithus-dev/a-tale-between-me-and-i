using System.Collections;
using UnityEngine;
using ATBMI.Entities.Player;

namespace ATBMI.Entities.NPCs
{
    public class TaskPush : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly float pushForce;
        private readonly float pushDelay;
        
        private PlayerController _player;
        
        // Constructor
        public TaskPush(CharacterAI character, float pushForce, float pushDelay)
        {
            this.character = character;
            this.pushForce = pushForce;
            this.pushDelay = pushDelay;
            
            InitFactors(planning: 1, risk: 0.7f, timeRange: (0.3f, 1f));
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;
            
            Vector2 direction = (_player.transform.position - character.transform.position).normalized;
            
            _player.StopMovement();
            _player.PlayerRb.AddForce(direction * pushForce, ForceMode2D.Impulse);
            _player.StartCoroutine(DonePushRoutine());
            
            return NodeStatus.Success;
        }

        private bool TrySetupTarget()
        {
            if (_player != null)
                return true;
            
            var target = (Collider2D)GetData(PHYSIC_KEY);
            if (!target)
            {
                Debug.LogWarning("Execute Failure: TaskPush");
                return false;
            }
            
            Debug.Log("Execute Success: TaskPush");
            _player = target.GetComponent<PlayerController>();
            return true;
        }
        
        private IEnumerator DonePushRoutine()
        {
            yield return new WaitForSeconds(pushDelay);
            _player.PlayerRb.velocity = Vector2.zero;
            _player.StartMovement();
            
            parentNode.ClearData(PHYSIC_KEY);
        }
    }
}