using UnityEngine;
using ATBMI.Entities;

namespace ATBMI.Cutscene
{
    public class AnimateCutscene : Cutscene
    {
        #region Fields
        
        [Header("Stats")]
        [SerializeField] private string animationState;
        [SerializeField] private Transform animateTransform;
        
        private bool _isPerformAnimation;
        private IAnimatable _animatable;
        
        #endregion

        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            _animatable = animateTransform.GetComponentInChildren<IAnimatable>();
        }

        public override void Execute()
        {
            if (_animatable == null)
            {
                Debug.LogWarning("animatable is null!");
                return;
            }
            
            _isPerformAnimation = _animatable.TrySetAnimationState(animationState);
        }

        public override bool IsFinished() => _isPerformAnimation;
        
        #endregion
    }
}