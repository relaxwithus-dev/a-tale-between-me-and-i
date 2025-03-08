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
            
            InitFactors(plan: 1, risk: 0.1f, timeRange: (0.15f, 4f));
        }
        
        public override NodeStatus Evaluate()
        {
            if (!TrySetupAnimation())
                return NodeStatus.Failure;
            
            _currentTime += Time.deltaTime;
            return _currentTime > _animationTime ? NodeStatus.Success : NodeStatus.Running;
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
            
            Debug.Log("Execute Success: TaskAnimate");
            _animationTime = animation.GetAnimationTime();
            return true;
        }
    }
}