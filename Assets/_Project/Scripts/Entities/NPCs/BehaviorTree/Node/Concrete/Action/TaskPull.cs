using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskPull : TaskForceBase
    {
        private readonly float holdForce = 0.9f;
        private readonly float holdTime = 0.3f;
        
        private bool _isHolding;
        private float _currentHoldTime;
        private Vector3 _holdDirection;
        private Vector3 _pullDirection;

        public TaskPull(CharacterAI character, float force, float delay) : base(character, force, delay)
        {
            InitFactors(plan: 1, risk: 0.7f, timeRange: (0.3f, 0.6f));
        }
        
        protected override NodeStatus PerformForce()
        {
            if (_currentHoldTime < holdTime)
            {
                if (!_isHolding)
                {
                    InitiateDirection();
                    HoldTarget();
                }
                
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
        
        private void InitiateDirection()
        {
            if (_pullDirection != Vector3.zero && _holdDirection != Vector3.zero)
                return;
            
            _pullDirection = character.transform.position - player.transform.position;
            _pullDirection.Normalize();
            
            _holdDirection = player.transform.position - character.transform.position;
            _holdDirection.Normalize();
        }
        
        private NodeStatus PullTarget()
        {
            player.PlayerRb.AddForce(_pullDirection * force, ForceMode2D.Impulse);
            player.StartCoroutine(WhenDoneForce());
            return NodeStatus.Success;
        }
        
        private void HoldTarget()
        {
            _isHolding = true; 
            character.LookAt(_pullDirection);
            
            player.PlayerFlip();
            player.StopMovement();
            player.PlayerRb.AddForce(_holdDirection * holdForce, ForceMode2D.Impulse);
        }
    }
}