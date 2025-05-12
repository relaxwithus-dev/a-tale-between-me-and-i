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
        [SerializeField] private Transform characterTransform;
        [SerializeField] private Transform targetPoint;
        
        private bool _isFinished;
        private EntitiesState _animationState;
        private Tweener _tween;
        private IController _controller;
        
        #endregion
        
        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            
            _isFinished = false;
            _animationState = isRunning ? EntitiesState.Run : EntitiesState.Walk;
            _controller = characterTransform.GetComponent<IController>();
        }
        
        public override void Execute()
        {
            var targetPosX = targetPoint.position.x;
            var lookPos = targetPoint.position - characterTransform.position;
            lookPos.Normalize();
            
            _tween?.Kill(false);
            
            _controller.LookAt(lookPos);
            _controller.ChangeState(_animationState);
            _tween = characterTransform.DOMoveX(targetPosX, moveDuration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    _controller.ChangeState(EntitiesState.Idle);
                    _isFinished = true;
                });
        }
        
        public override bool IsFinished() => _isFinished;
        
        #endregion
    }
}