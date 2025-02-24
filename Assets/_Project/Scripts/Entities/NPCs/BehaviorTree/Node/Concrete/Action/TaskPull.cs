using System.Collections;
using UnityEngine;
using ATBMI.Entities.Player;

namespace ATBMI.Entities.NPCs
{
    public class TaskPull : LeafWeight
    {
        private readonly CharacterAI character;
        private readonly float pullForce;
        private readonly float pullDelay;
        private readonly float holdForce = 0.9f;
        private readonly float holdTime = 0.3f;
        
        private bool _isHolding;
        private float _currentHoldTime;
        private PlayerController _player;
        
        public TaskPull(CharacterAI character, float pullForce, float pullDelay)
        {
            this.character = character;
            this.pullForce = pullForce;
            this.pullDelay = pullDelay;
            
            InitFactors(planning: 1, risk: 0.8f, timeRange: (0.5f, 1.5f));
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupTarget())
                return NodeStatus.Failure;
            
            if (_currentHoldTime < holdTime)
            {
                if (!_isHolding)
                    HoldTarget();
                
                _currentHoldTime += Time.deltaTime;
                return NodeStatus.Running;
            }
            
            return PullTarget();
        }
        
        protected override void Reset()
        {
            base.Reset();
            _isHolding = false;
        }
        
        private NodeStatus PullTarget()
        {
            Vector2 direction = (character.transform.position - _player.transform.position).normalized;
            
            _player.PlayerRb.AddForce(direction * pullForce, ForceMode2D.Impulse);
            _player.StartCoroutine(DonePullRoutine());
            return NodeStatus.Success;
        }

        private void HoldTarget()
        {
            Debug.LogWarning("Attempting to hold target");
            Vector2 direction = (_player.transform.position - character.transform.position).normalized;
            
            _isHolding = true;
            _player.StopMovement();
            _player.PlayerRb.AddForce(direction * holdForce, ForceMode2D.Impulse);
        }
        
        private bool TrySetupTarget()
        {
            if (_player != null)
                return true;
            
            var target = (Collider2D)GetData(PHYSIC_KEY);
            if (!target)
            {
                Debug.LogWarning("Execute Failure: TaskPull");
                return false;
            }
            
            Debug.Log("Execute Success: TaskPull");
            _player = target.GetComponent<PlayerController>();
            return true;
        }
        
        private IEnumerator DonePullRoutine()
        {
            yield return new WaitForSeconds(pullDelay);
            _player.PlayerRb.velocity = Vector2.zero;
            _player.StartMovement();
            
            parentNode.ClearData(PHYSIC_KEY);
        }
    }
}