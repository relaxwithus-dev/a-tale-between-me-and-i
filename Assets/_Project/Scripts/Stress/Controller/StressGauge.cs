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
        [SerializeField] private Gradient fullFillGradient;
        [SerializeField] private Sprite[] gaugeIconSprites;
        
        private float _currentStressValue;
        private float _increasedValue;
        private bool _isStatusActive;
  
        private Coroutine _overtimeRoutine;
        private readonly List<float> _fillAmountValues = new();
        
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
            InitFillAmountValue();
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
            gaugeImageUI.color = fullFillGradient.colorKeys[0].color;
            gaugeIconUI.sprite = gaugeIconSprites[0];
        }
        
        private void InitFillAmountValue()
        {
            var iconLenght = gaugeIconSprites.Length;
            var valueDivider = MAX_SLIDER_VALUE / iconLenght;
            for (var i = iconLenght - 1; i > 0; i--)
            {
                _fillAmountValues.Add(i * valueDivider);
            }
        }
        
        // Core
        private void IncreaseGauge()
        {
            var currentValue = _currentStressValue / stressValue;
            
            gaugeImageUI.fillAmount = Mathf.Lerp(gaugeImageUI.fillAmount, currentValue, Time.deltaTime / gaugeDuration);
            gaugeImageUI.color = fullFillGradient.Evaluate(gaugeImageUI.fillAmount);
            
            if (gaugeImageUI.fillAmount > MAX_SLIDER_VALUE)
            {
                _isStatusActive = true;
                gaugeImageUI.fillAmount = MAX_SLIDER_VALUE;
                gaugeImageUI.color = fullFillGradient.colorKeys[^1].color;
                
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
                gaugeImageUI.color = fullFillGradient.Evaluate(gaugeImageUI.fillAmount);
                
                HandleGaugeIcon(gaugeImageUI.fillAmount);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            InitGaugeStats();
            PlayerEvents.StressInactiveEvent();
        }
        
        private void StressOnce(bool isIncrease, float value = 0)
        {
            if (!isIncrease)
            {
                var decrease = Random.Range(1f, 65f);
                value += decrease * -1f;
            }
            
            _isStatusActive = false;
            _currentStressValue += value;
            _currentStressValue = Mathf.Clamp(_currentStressValue, 0f, 100f);
            if (_currentStressValue >= stressValue)
            {
                _isStatusActive = true;
                _currentStressValue = stressValue;
            }
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
        
        private void HandleGaugeIcon(float value)
        {
            gaugeIconUI.sprite = value switch
            {
                _ when value >= _fillAmountValues[2] => gaugeIconSprites[3],
                _ when value >= _fillAmountValues[1] => gaugeIconSprites[2],
                _ when value >= _fillAmountValues[0] => gaugeIconSprites[1],
                _ => gaugeIconSprites[0]
            };
        }
        
        #endregion
    }
}