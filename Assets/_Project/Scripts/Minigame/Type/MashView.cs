using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Minigame
{
    public class MashView : MinigameView
    {
        #region MashData

        [Serializable]
        private struct MashAttribute
        {
            public float increase;
            public float decrease;
            public float delay;
        }

        #endregion
        
        #region Fields & Properties

        [Header("Attribute")]
        [SerializeField] [Range(0, 100)] private float mashMaxValue;
        [SerializeField] [MaxValue(100)] private float initiateMashValue;
        [SerializeField] private MashAttribute[] mashAttributes;
        
        private int _mashCount;
        private float _currentValue;
        private float _elapsedTime;
        
        private MashAttribute _attribute;
        
        [Header("Tween")]
        [SerializeField] private float sliderSpeed;
        [SerializeField] private Vector3 indicatorTargetScale;
        [SerializeField] private Vector3 indicatorOriginalScale;
        [SerializeField] private float indicatorTime;
        
        [Header("UI")]
        [SerializeField] private Slider mashSliderUI;
        [SerializeField] private TextMeshProUGUI valueTextUI;
        [SerializeField] private TextMeshProUGUI targetValueTextUI;
        [SerializeField] private RectTransform[] indicatorRect;
        
        // Reference
        private GameInputHandler _inputHandler;
        
        #endregion
        
        #region Methods
        
        // Initialize
        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            _inputHandler = GameInputHandler.Instance;
        }
        
        // Core
        public override void EnterMinigame() 
        {
            base.EnterMinigame();
                        
            _mashCount = 0;
            _elapsedTime = 0f;
            _currentValue = initiateMashValue;
            _attribute = mashAttributes[playingCount];
            
            mashSliderUI.value = _currentValue / mashMaxValue;
            indicatorRect[_mashCount].localScale = indicatorTargetScale;
        }
        
        protected override void RunMinigame()
        {
            base.RunMinigame();
            ModifySliderValue();
            ModifyTextValue();

            if (_currentValue <= 0) return;
            HandleIncreaseMash();
            HandleDecreaseMash();
        }
        
        protected override void ExitMinigame()
        {
            base.ExitMinigame();
            mashSliderUI.value =  initiateMashValue / mashMaxValue;
            foreach (var rect in indicatorRect)
            {
                rect.localScale = Vector3.one;
            }
        }
        
        private void HandleIncreaseMash()
        {
            switch (_mashCount)
            {
                case 0 when _inputHandler.IsArrowRight:
                    _mashCount = 1;
                    ModifyIndicator();
                    break;
                case 1 when _inputHandler.IsArrowLeft:
                    _mashCount = 0;
                    _currentValue += _attribute.increase;
                    ModifyIndicator();
                    break;
            }
        }
        
        private void HandleDecreaseMash()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _attribute.delay)
            {
                _elapsedTime = 0f;
                _currentValue -= _attribute.decrease;
                _currentValue = Mathf.Max(_currentValue, 0f);
            }
        }
        
        private void ModifySliderValue()
        {
            var sliderValue = _currentValue / mashMaxValue;
            mashSliderUI.value = Mathf.Lerp(mashSliderUI.value, sliderValue, sliderSpeed * Time.deltaTime);
            
            switch (sliderValue)
            {
                // Win
                case >= MAX_SLIDER_VALUE:
                    playingCount = Mathf.Clamp(playingCount + 1, 0, mashAttributes.Length - 1);
                    ModifyIndicator(isEnd: true);
                    ExitMinigame();
                    break;
                // Lose
                case <= MIN_SLIDER_VALUE:
                    ModifyIndicator(isEnd: true);
                    ExitMinigame();
                    break;
            }
        }
        
        private void ModifyTextValue()
        {
            valueTextUI.text = _currentValue + "%";
            targetValueTextUI.text = mashMaxValue - _currentValue + "%";
        }
        
        private void ModifyIndicator(bool isEnd = false)
        {
            for (var i = 0; i < indicatorRect.Length; i++)
            {
                var isActive = !isEnd && i == _mashCount;
                AnimateIndicator(i, isActive);
            }
        }
        
        private void AnimateIndicator(int index, bool isScaleUp)
        {
            var scale = isScaleUp ? indicatorTargetScale : indicatorOriginalScale;
            var ease = isScaleUp ? Ease.InSine : Ease.OutSine;
            
            indicatorRect[index].DOScale(scale, indicatorTime).SetEase(ease);
        }
        
        #endregion
    }
}