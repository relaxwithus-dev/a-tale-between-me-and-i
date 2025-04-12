using UnityEngine;
using DG.Tweening;

namespace ATBMI.Minigame
{
    public class MashIndicatorHandler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")] 
        [SerializeField] private Vector3 targetScale;
        [SerializeField] private Vector3 originalScale;
        [SerializeField] private float tweenTime;

        private int _initRectIndex = 0;
        
        [Header("UI")]
        [SerializeField] private RectTransform[] indicatorRect;
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void OnEnable()
        {
            for (var i = 0; i < indicatorRect.Length; i++)
            {
                var scale = i == _initRectIndex ? targetScale : originalScale;
                indicatorRect[i].localScale = scale;
            }
        }
        
        // Core
        public void ModifyIndicator(int mashCount, bool isEnd = false)
        {
            if (mashCount > indicatorRect.Length - 1)
            {
                Debug.LogWarning("indicator count is greater than the amount of mashes");
                return;
            }
            
            for (var i = 0; i < indicatorRect.Length; i++)
            {
                var isActive = !isEnd && i == mashCount;
                AnimateIndicator(i, isActive);
            }
        }
        
        private void AnimateIndicator(int index, bool isScaleUp)
        {
            var scale = isScaleUp ? targetScale : originalScale;
            var ease = isScaleUp ? Ease.InSine : Ease.OutSine;
            
            indicatorRect[index].DOScale(scale, tweenTime).SetEase(ease);
        }

        #endregion
    }
}