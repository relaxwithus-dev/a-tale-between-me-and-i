using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Minigame
{
    public class MashView : MinigameView
    {
        #region Fields & Properties

        [Header("Attribute")]
        [SerializeField] [Range(0, 100)] private float mashMaxValue;
        [SerializeField] [MaxValue(100)] private float initiateMashValue;
        [SerializeField] private float mashIncrease;
        [SerializeField] private float mashDecrease;
        [SerializeField] private float decreaseDelay;
        [SerializeField] private float lerpSpeed;
        
        private int _mashCount;
        private float _currentValue;
        private float _elapsedTime;
        
        [Header("UI")]
        [SerializeField] private Slider mashSliderUI;
        [SerializeField] private TextMeshProUGUI valueTextUI;
        [SerializeField] private TextMeshProUGUI targetValueTextUI;
        [SerializeField] private Image[] indicatorImages;
        
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
            
            mashSliderUI.value = _currentValue / mashMaxValue;
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
        }
        
        private void HandleIncreaseMash()
        {
            if (_inputHandler.IsArrowRight && _mashCount == 0)
            {
                _mashCount++;
            }
            else if (_inputHandler.IsArrowLeft && _mashCount == 1)
            {
                _mashCount--;
                _currentValue += mashIncrease;
            }
        }
        
        private void HandleDecreaseMash()
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= decreaseDelay)
            {
                _elapsedTime = 0f;
                _currentValue -= mashDecrease;
                _currentValue = Mathf.Max(_currentValue, 0f);
            }
        }
        
        private void ModifySliderValue()
        {
            var sliderValue = _currentValue / mashMaxValue;
            mashSliderUI.value = Mathf.Lerp(mashSliderUI.value, sliderValue, lerpSpeed / Time.deltaTime);
            
            switch (sliderValue)
            {
                // Win
                case >= MAX_SLIDER_VALUE:
                    ExitMinigame();
                    break;
                // Lose
                case <= MIN_SLIDER_VALUE:
                    ExitMinigame();
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