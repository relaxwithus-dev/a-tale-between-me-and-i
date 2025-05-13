using UnityEngine;
using DG.Tweening;
using ATBMI.Entities;
using Sirenix.OdinInspector;

namespace ATBMI.Cutscene
{
    public class MoveCutscene : Cutscene
    {
        #region Fields
        
        private enum TargetType { Player, Character }
        
        [Header("Stats")]
        [SerializeField] private TargetType type;
        [SerializeField] private bool isRunning;
        [SerializeField] private float moveDuration;
        
        private bool _isFinished;
        
        [Header("Animation")]
        [SerializeField] private Ease easeType;
        [SerializeField] [ShowIf("type", TargetType.Character)]
        private Transform characterTransform;
        [SerializeField] private Transform targetPoint;
        
        private EntitiesState _animationState;
        private Tweener _tween;
        private IController _controller;
        
        [Header("Reference")]
        [SerializeField] private CutsceneManager cutsceneManager;
        
        #endregion
        
        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            
            _isFinished = false;
            _animationState = isRunning ? EntitiesState.Run : EntitiesState.Walk;

            if (type == TargetType.Player)
            {
                characterTransform = cutsceneManager.PlayerTransform;
            }
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