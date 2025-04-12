using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

using Random = UnityEngine.Random;

namespace ATBMI.Minigame
{
    public class TimingBarController : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Properties")] 
        [SerializeField] private RectTransform[] barRestrictions;
        
        private float _currentSpeed;
        private int _initiatePosIndex;
        
        private Tween _moveTween;
        private RectTransform _timingBarRect;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            _timingBarRect = GetComponent<RectTransform>();
        }
        
        // Core
        public void StartAnimateBar((float min, float max) speedRange)
        {
            // Initiate
            _currentSpeed = Random.Range(speedRange.min, speedRange.max);
            _initiatePosIndex = Random.Range(0, barRestrictions.Length);
            _timingBarRect.anchoredPosition = new Vector2(barRestrictions[_initiatePosIndex].anchoredPosition.x, 
                _timingBarRect.anchoredPosition.y);
            
            // Move
            MoveTimingBar();
        }
        
        public IEnumerator PauseWithSeconds(float seconds, Action callback = null)
        {
            _moveTween.Pause();
            yield return new WaitForSeconds(seconds);
            
            _moveTween.Play();
            callback?.Invoke();
        }
        
        public void StopTimingBar()
        {
            _moveTween.Kill(false);
            _initiatePosIndex = 0;
        }
        
        private void MoveTimingBar()
        {
            _moveTween?.Kill(false);
            
            var moveSequence = DOTween.Sequence();
            for (var i = 0; i < barRestrictions.Length; i++)
            {
                var targetIndex = Mathf.Abs((_initiatePosIndex + i - 1) % barRestrictions.Length);
                moveSequence.Append(_timingBarRect.DOAnchorPosX(barRestrictions[targetIndex]
                        .anchoredPosition.x, _currentSpeed)
                    .SetEase(Ease.Linear));
            }
            
            _moveTween = moveSequence;
            _moveTween.SetEase(Ease.Linear)
                .SetRelative(false)
                .SetLoops(-1, LoopType.Yoyo);
        }
        
        #endregion
    }
}