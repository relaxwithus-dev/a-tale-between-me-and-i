using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskAnimate : LeafWeight
    {
        private readonly CharacterAnimation animation;
        private readonly string state;
        
        private float _currentTime;
        private float _animationTime;

        public TaskAnimate(CharacterAnimation animation, string state)
        {
            this.animation = animation;
            this.state = state;
            
            InitFactors(plan: 1, risk: 0.3f, timeRange: (0.35f, 4f));
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupAnimation())
                return NodeStatus.Failure;
            
            _currentTime += Time.deltaTime;
            if (_currentTime > _animationTime)
            {
                Debug.Log("Execute Success: TaskAnimate");
                return NodeStatus.Success;
            }
            
            return NodeStatus.Running;
        }
        
        protected override void Reset()
        {
            base.Reset();
            _currentTime = 0f;
            _animationTime = 0f;
        }

        private bool TrySetupAnimation()
        {
            if (_animationTime != 0f)
                return true;
            
            if (!animation.TrySetAnimationState(state))
            {
                Debug.LogWarning("Execute Failure: TaskAnimate");
                return false;
            }
            
            _animationTime = animation.GetAnimationTime();
            return true;
        }
    }
}