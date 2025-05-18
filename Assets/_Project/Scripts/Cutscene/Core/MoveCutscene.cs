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
        
        [Header("Attribute")]
        [SerializeField] private TargetType targetType;
        [SerializeField] private bool isRunning;
        
        [Header("Animation")]
        [SerializeField] private Ease easeType;
        [SerializeField] private float moveDuration;
        [SerializeField] [ShowIf("targetType", TargetType.Character)]
        private Transform targetTransform;
        [SerializeField] private Transform targetPoint;
        
        private bool _isFinished;
        private EntitiesState _animationState;
        private IController _iController;
        private Tweener _tween;
        
        #endregion
        
        #region Methods
        
        protected override void InitOnStart()
        {
            base.InitOnStart();
            
            _isFinished = false;
            _animationState = isRunning ? EntitiesState.Run : EntitiesState.Walk;
            
            if (targetType == TargetType.Player)
                targetTransform = CutsceneManager.Instance.Player;
            _iController = targetTransform.GetComponent<IController>();
        }
        
        public override void Execute()
        {
            var targetPosX = targetPoint.position.x;
            var lookPos = targetPoint.position - targetTransform.position;
            lookPos.Normalize();
            
            _tween?.Kill(false);
            
            _iController.LookAt(lookPos);
            _iController.ChangeState(_animationState);
            _tween = targetTransform.DOMoveX(targetPosX, moveDuration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    _iController.ChangeState(EntitiesState.Idle);
                    _isFinished = true;
                });
        }
        
        public override bool IsFinished() => _isFinished;
        
        #endregion
    }
}