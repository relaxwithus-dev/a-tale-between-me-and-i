using UnityEngine;
using DG.Tweening;
using ATBMI.Entities;

namespace ATBMI.Cutscene
{
    public class MoveCutscene : Cutscene
    {
        #region Fields
        
        [Header("Stats")] 
        [SerializeField] private Ease easeType;
        [SerializeField] private bool isRunning;
        [SerializeField] private float moveDuration;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Transform targetPoint;

        private bool _isFinished;
        private string _animationState;
        private Tweener _tween;
        private IAnimatable _animatable;
        
        #endregion

        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            
            _isFinished = false;
            _animationState = isRunning ? "Run" : "Walk";
            _animatable = targetTransform.GetComponent<IAnimatable>();
        }
        
        public override void Execute()
        {
            var targetPosX = targetPoint.position.x;
            
            _tween?.Kill(false);
            _animatable.TrySetAnimationState(_animationState);
            _tween = targetTransform.DOMoveX(targetPosX, moveDuration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    _animatable.TrySetAnimationState("Idle");
                    _isFinished = true;
                });
        }

        public override bool IsFinished() => _isFinished;
        
        #endregion
    }
}