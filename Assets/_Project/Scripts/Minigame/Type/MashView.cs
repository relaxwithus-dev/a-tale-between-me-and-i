using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace ATBMI.Minigame
{
    public class MashView : MinigameView
    {
        #region Struct

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
        [SerializeField] private float sliderSpeed;
        [SerializeField] private MashAttribute[] mashAttributes;
        
        private int _mashCount;
        private float _currentValue;
        private float _elapsedTime;
        
        private MashAttribute _attribute;
        
        [Header("UI")]
        [SerializeField] private Slider mashSliderUI;
        [SerializeField] private TextMeshProUGUI valueTextUI;
        [SerializeField] private TextMeshProUGUI targetValueTextUI;
        
        // Reference
        [Header("Reference")]
        [SerializeField] private MashIndicatorHandler indicatorHandler;
        
        #endregion
        
        #region Methods
        
        // Core
        public override void EnterMinigame() 
        {
            base.EnterMinigame();
                        
            _mashCount = 0;
            _elapsedTime = 0f;
            _currentValue = initiateMashValue;
            _attribute = mashAttributes[playingCount];
            
            mashSliderUI.value = _currentValue / mashMaxValue;
        }
        
        protected override void RunMinigame()
        {
            base.RunMinigame();
            ModifySliderValue();
            ModifyTextValue();

            if (_currentValue <= 0 || _currentValue >= mashMaxValue) return;
            HandleIncreaseMash();
            HandleDecreaseMash();
        }
        
        private void HandleIncreaseMash()
        {
            switch (_mashCount)
            {
                case 0 when inputHandler.IsArrowRight:
                    _mashCount = 1;
                    indicatorHandler.ModifyIndicator(_mashCount);
                    break;
                case 1 when inputHandler.IsArrowLeft:
                    _mashCount = 0;
                    _currentValue += _attribute.increase;
                    _currentValue = Mathf.Min(_currentValue, mashMaxValue);
                    indicatorHandler.ModifyIndicator(_mashCount);
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
            
            switch (mashSliderUI.value)
            {
                // Win
                case >= MAX_SLIDER_VALUE:
                    playingCount = Mathf.Clamp(playingCount + 1, 0, mashAttributes.Length - 1);
                    indicatorHandler.ModifyIndicator(_mashCount, true);
                    mashSliderUI.value = MAX_SLIDER_VALUE;
                    ExitMinigame(isWinning: true);
                    break;
                // Lose
                case <= MIN_SLIDER_VALUE:
                    indicatorHandler.ModifyIndicator(_mashCount, true);
                    mashSliderUI.value = MIN_SLIDER_VALUE;
                    ExitMinigame(isWinning: false);
                    break;
            }
        }
        
        private void ModifyTextValue()
        {
            valueTextUI.text = _currentValue + "%";
            targetValueTextUI.text = mashMaxValue - _currentValue + "%";
        }
        
        #endregion
    }
}