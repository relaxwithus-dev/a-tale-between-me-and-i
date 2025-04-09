using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TaskAnimate : LeafWeight
    {
        private readonly CharacterAnimation animation;
        private readonly string state;
        
        private float _currentTime;
        private float _animationTime;
        
        private readonly Dictionary<Emotion, (float plan, float risk, (float, float) time)> _factorsAnimate = new()
        {
            { Emotion.Joy, (1, 0.2f, (1.5f, 3f)) },
            { Emotion.Trust, (1, 0.4f, (1.15f, 4.6f)) },
            { Emotion.Fear, (1, 0.2f, (1.5f, 5f)) },
            { Emotion.Surprise, (1, 0.4f, (0.35f, 3.7f)) },
            { Emotion.Sadness, (1, 0.2f, (0.35f, 4f)) },
            { Emotion.Disgust, (1, 0.2f, (0.35f, 4f)) },
            { Emotion.Anger, (1, 0.2f, (3f, 6.5f)) },
            { Emotion.Anticipation, (1, 0.3f, (0.35f, 4f)) }
        };
        
        // Constructor
        public TaskAnimate(CharacterAnimation animation, string state)
        {
            this.animation = animation;
            this.state = state;
            
            OverrideEmotionFactors(_factorsAnimate);
        }

        // Core
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