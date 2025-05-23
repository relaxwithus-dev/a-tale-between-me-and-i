using System;
using UnityEngine;
using DG.Tweening;

namespace ATBMI.Minigame
{
    public class MinigameAnimation : MonoBehaviour
    {
        #region Fields
        
        [Header("Animation")] 
        [SerializeField] private Ease openEase;
        [SerializeField] private Ease closeEase;
        
        [SerializeField] private float tweenDuration;
        [SerializeField] private float bounceDuration;
        [SerializeField] private Vector3 targetScale;
        [SerializeField] private Vector3 originalScale;

        // Reference
        private RectTransform _rectTransform;

        #endregion
        
        #region Methods
        
        // Core
        public void OpenMinigame(Action onComplete)
        {
            if (!_rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            
            _rectTransform.localScale = Vector3.zero;
            _rectTransform.DOScale(targetScale, tweenDuration).SetEase(openEase);
            _rectTransform.DOScale(originalScale, bounceDuration)
                .SetEase(openEase)
                .SetDelay(tweenDuration)
                .OnComplete(() => onComplete?.Invoke());
        }
        
        public void CloseMinigame(Action onComplete)
        {
            _rectTransform.localScale = originalScale;
            _rectTransform.DOScale(targetScale, bounceDuration).SetEase(closeEase);
            _rectTransform.DOScale(Vector3.zero, tweenDuration)
                .SetEase(closeEase)
                .SetDelay(bounceDuration)
                .OnComplete(() => onComplete?.Invoke());
        }
        
        #endregion
    }
}