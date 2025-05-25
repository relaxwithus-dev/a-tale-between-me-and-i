using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ATBMI.Minigame
{
    public class TimingView : MinigameView
    {
        #region Struct

        [Serializable]
        private struct TimingAttribute
        {
            public float duration;
            public float minSpeed;
            public float maxSpeed;
        }

        #endregion
        
        #region Fields & Properties

        [Header("Attribute")] 
        [SerializeField] private float wrongDelayTime;
        [SerializeField] private TimingAttribute[] timingAttributes;
        [SerializeField] private RectTransform[] hitAreaRestrictions = new RectTransform[2];
        
        private bool _canInteract;
        private float _elapsedTime;
        private TimingAttribute _attribute;
        
        [Header("UI")] 
        [SerializeField] private RectTransform hitAreaRect;
        [SerializeField] private RectTransform timingBarRect;
        [SerializeField] private Slider timeSliderUI;
        
        // Reference
        private TimingBarController _timingController;
        
        #endregion
        
        #region Methods
        
        // Initialize
        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            _timingController = timingBarRect.GetComponent<TimingBarController>();
        }
        
        // Core
        public override void EnterMinigame() 
        {
            base.EnterMinigame();
            
            _elapsedTime = 0f;
            _canInteract = true;
            _attribute = timingAttributes[playingCount];
            _timingController.StartAnimateBar((_attribute.minSpeed, _attribute.maxSpeed));
            
            // Random area anchoredPosition
            var minX = hitAreaRestrictions[0].anchoredPosition.x;
            var maxX = hitAreaRestrictions[1].anchoredPosition.x;
            hitAreaRect.anchoredPosition = new Vector2(Random.Range(minX, maxX), hitAreaRect.anchoredPosition.y);
        }
        
        protected override void RunMinigame()
        {
            base.RunMinigame();
            HandleBalanceGameplay();
            HandleBalanceTime();
        }
        
        private void HandleBalanceGameplay()
        {
            if (!_canInteract) return;
            
            if (inputHandler.IsTapInteract)
            {
                var hitAreaWorld = WorldRect(hitAreaRect);
                var timingBarWorld = WorldRect(timingBarRect);
                
                // Win
                if (hitAreaWorld.Overlaps(timingBarWorld))
                {
                    playingCount = Mathf.Clamp(playingCount + 1, 0, timingAttributes.Length - 1);
                    _timingController.StopTimingBar();
                    ExitMinigame(isWinning: true);
                }
                else
                {
                    _canInteract = false;
                    StartCoroutine(_timingController.PauseWithSeconds(wrongDelayTime, () =>
                    {
                        _canInteract = true;
                    }));
                }
            }
        }
        
        private Rect WorldRect(RectTransform rectTransform)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            Vector2 pivot = rectTransform.pivot;
            
            // Check rect widht/height
            var rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            var rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            // Check rect position
            Vector3 position = rectTransform.TransformPoint(rectTransform.rect.center);
            var x = position.x - rectTransformWidth * 0.5f;
            var y = position.y - rectTransformHeight * 0.5f;
            
            return new Rect(x,y, rectTransformWidth, rectTransformHeight);
        }
        
        private void HandleBalanceTime()
        {
            _elapsedTime += Time.deltaTime;
            timeSliderUI.value = Mathf.Lerp(MAX_SLIDER_VALUE, MIN_SLIDER_VALUE, _elapsedTime / _attribute.duration);
            
            // Lose
            if (timeSliderUI.value <= MIN_SLIDER_VALUE)
            {
                timeSliderUI.value = MIN_SLIDER_VALUE;
                ExitMinigame(isWinning: false);
            }
        }
        
        #endregion
    }
}