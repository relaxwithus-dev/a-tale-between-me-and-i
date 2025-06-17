using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ATBMI.Gameplay.Event;

namespace ATBMI.Stress
{
    public class StressGauge : MonoBehaviour
    {
        #region Fields & Property

        [Header("Stats")]
        [SerializeField] private float stressValue = 100f;
        [SerializeField] private float statusDuration;
        [SerializeField] private float gaugeDuration;
        [SerializeField] private float increasedPercent;
        [SerializeField] [Range(0.1f, 1f)] private float increasedInterval;
        
        [Space] 
        [SerializeField] private Color fullFillColor;
        [SerializeField] private Color endFillColor;
        [SerializeField] private Sprite[] gaugeIconSprites;
        
        private float _currentStressValue;
        private float _increasedValue;
        private List<float> _sliderValues = new();
        
        private bool _isStatusActive;
        private Coroutine _overtimeRoutine;
        
        private const float MAX_SLIDER_VALUE = 1f;
        private const float MIN_SLIDER_VALUE = 0f;
        
        [Header("UI")]
        [SerializeField] private Image gaugeImageUI;
        [SerializeField] private Image gaugeIconUI;
        
        #endregion

        #region Methods

        // Unity Callbacks
        private void OnEnable()
        {
            StressEvents.OnStressOnce += StressOnce;
            StressEvents.OnStressOvertime += StressOvertime;
        }

        private void OnDisable()
        {
            StressEvents.OnStressOnce -= StressOnce;
            StressEvents.OnStressOvertime -= StressOvertime;
        }
        
        private void Start()
        {
            InitGaugeStats();
            
            var iconLenght = gaugeIconSprites.Length;
            var valueDivider = MAX_SLIDER_VALUE / iconLenght;
            for (var i = iconLenght - 1; i > 0; i--)
            {
                _sliderValues.Add(i * valueDivider);
            }
        }
        
        private void Update()
        {
            if (_isStatusActive) return;
            
            IncreaseGauge();
            HandleGaugeIcon(gaugeImageUI.fillAmount);
        }
        
        // Initialize
        private void InitGaugeStats()
        {
            _isStatusActive = false;
            _currentStressValue = MIN_SLIDER_VALUE;
            _increasedValue = stressValue * (increasedPercent / 100f);
            
            gaugeImageUI.fillAmount = MIN_SLIDER_VALUE;
            gaugeImageUI.color = endFillColor;
            gaugeIconUI.sprite = gaugeIconSprites[0];
        }
        
        // Core
        private void StressOnce(bool condition, float value)
        {
            _isStatusActive = false;

            if (!condition)
                value *= -1f;
            _currentStressValue += value;
        }
        
        private void StressOvertime(bool condition)
        {
            if (condition && _overtimeRoutine == null)
            {
                _overtimeRoutine = StartCoroutine(StressOvertimeRoutine());
            }
            else if (!condition && _overtimeRoutine != null)
            {
                StopCoroutine(_overtimeRoutine);
                _overtimeRoutine = null;
            }
        }
        
        private IEnumerator StressOvertimeRoutine()
        {
            _isStatusActive = false;
            while (_currentStressValue < stressValue)
            {
                _currentStressValue += _increasedValue;
                yield return new WaitForSeconds(increasedInterval);
            }
            
            _currentStressValue = stressValue;
        }

        private void IncreaseGauge()
        {
            var currentValue = _currentStressValue / stressValue;
            gaugeImageUI.fillAmount = Mathf.Lerp(currentValue, MAX_SLIDER_VALUE, gaugeDuration);
            gaugeImageUI.color = Color.Lerp(endFillColor, fullFillColor, gaugeDuration);
            
            if (gaugeImageUI.fillAmount > MAX_SLIDER_VALUE)
            {
                _isStatusActive = true;
                gaugeImageUI.fillAmount = MAX_SLIDER_VALUE;
                gaugeImageUI.color = fullFillColor;
                
                // Activate stress effect
                StartCoroutine(DecreaseGaugeRoutine());
            }
        }
        
        private IEnumerator DecreaseGaugeRoutine()
        {
            var elapsedTime = 0f;
            PlayerEvents.StressActiveEvent();
            
            while (elapsedTime < statusDuration)
            {
                var time = elapsedTime / statusDuration;
                gaugeImageUI.fillAmount = Mathf.Lerp(MAX_SLIDER_VALUE, MIN_SLIDER_VALUE, time);
                gaugeImageUI.color = Color.Lerp(endFillColor, fullFillColor, time);
                
                HandleGaugeIcon(gaugeImageUI.fillAmount);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            InitGaugeStats();
            PlayerEvents.StressInactiveEvent();
        }
        
        private void HandleGaugeIcon(float value)
        {
            gaugeIconUI.sprite = value switch
            {
                _ when value >= _sliderValues[2] => gaugeIconSprites[3],
                _ when value >= _sliderValues[1] => gaugeIconSprites[2],
                _ when value >= _sliderValues[0] => gaugeIconSprites[1],
                _ => gaugeIconSprites[0]
            };
        }
        
        #endregion
    }
}