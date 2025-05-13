using UnityEngine;
using DG.Tweening;
using ATBMI.Entities;

namespace ATBMI.Cutscene
{
    public class MoveCutscene : Cutscene
    {
        #region Fields
        
        [Header("Stats")]
        [SerializeField] private bool isRunning;
        [SerializeField] private Ease easeType;
        [SerializeField] private float moveDuration;
        [SerializeField] private Transform targetPoint;
        
        private bool _isFinished;
        private EntitiesState _animationState;
        private Tweener _tween;
        
        #endregion
        
        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            
            _isFinished = false;
            _animationState = isRunning ? EntitiesState.Run : EntitiesState.Walk;
        }
        
        public override void Execute()
        {
            var targetPosX = targetPoint.position.x;
            var lookPos = targetPoint.position - targetTransform.position;
            lookPos.Normalize();
            
            _tween?.Kill(false);
            
            controller.LookAt(lookPos);
            controller.ChangeState(_animationState);
            _tween = targetTransform.DOMoveX(targetPosX, moveDuration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    controller.ChangeState(EntitiesState.Idle);
                    _isFinished = true;
                });
        }
        
        public override bool IsFinished() => _isFinished;
        
        #endregion
    }
}