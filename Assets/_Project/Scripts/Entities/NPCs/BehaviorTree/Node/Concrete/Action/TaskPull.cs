using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskPull : TaskForceBase
    {
        private readonly float holdForce = 0.9f;
        private readonly float holdTime = 0.3f;
        
        private bool _isHolding;
        private float _currentHoldTime;

        public TaskPull(CharacterAI character, float force, float delay) : base(character, force, delay)
        {
            InitFactors(plan: 1, risk: 0.8f, timeRange: (0.3f, 0.6f));
        }
        
        protected override NodeStatus PerformForce()
        {
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
            _currentHoldTime = 0f;
        }
        
        private NodeStatus PullTarget()
        {
            Vector2 direction = (character.transform.position - player.transform.position).normalized;
            
            player.PlayerRb.AddForce(direction * force, ForceMode2D.Impulse);
            player.StartCoroutine(WhenDoneForce());
            return NodeStatus.Success;
        }

        private void HoldTarget()
        {
            Vector2 direction = (player.transform.position - character.transform.position).normalized;
            
            _isHolding = true;
            player.StopMovement();
            player.PlayerRb.AddForce(direction * holdForce, ForceMode2D.Impulse);
        }
    }
}