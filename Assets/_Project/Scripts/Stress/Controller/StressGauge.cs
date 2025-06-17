using System.Collections;
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
        
        private float _currentStressValue;
        private float _increasedValue;
        private bool _isStatusActive;
        private Coroutine _overtimeRoutine;
        
        private const float MAX_SLIDER_VALUE = 1f;
        private const float MIN_SLIDER_VALUE = 0f;

        [Header("UI")]
        [SerializeField] private Image gaugeImageUI;
        
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
        }
        
        private void Update()
        {
            if (_isStatusActive) return;
            
            var currentValue = _currentStressValue / stressValue;
            gaugeImageUI.fillAmount = Mathf.Lerp(currentValue, MAX_SLIDER_VALUE, gaugeDuration);
            gaugeImageUI.color = Color.Lerp(endFillColor, fullFillColor, gaugeDuration);
            
            if (gaugeImageUI.fillAmount > MAX_SLIDER_VALUE)
            {
                _isStatusActive = true;
                gaugeImageUI.fillAmount = MAX_SLIDER_VALUE;
                gaugeImageUI.color = fullFillColor;
                
                // Activate stress effect
                StartCoroutine(DecreaseSliderRoutine());
            }
        }
        
        // Initialize
        private void InitGaugeStats()
        {
            _isStatusActive = false;
            _currentStressValue = MIN_SLIDER_VALUE;
            _increasedValue = stressValue * (increasedPercent / 100f);
            
            gaugeImageUI.fillAmount = MIN_SLIDER_VALUE;
            gaugeImageUI.color = endFillColor;
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
        
        private IEnumerator DecreaseSliderRoutine()
        {
            var elapsedTime = 0f;
            PlayerEvents.StressActiveEvent();
            
            while (elapsedTime < statusDuration)
            {
                var time = elapsedTime / statusDuration;
                gaugeImageUI.fillAmount = Mathf.Lerp(MAX_SLIDER_VALUE, MIN_SLIDER_VALUE, time);
                gaugeImageUI.color = Color.Lerp(endFillColor, fullFillColor, time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            InitGaugeStats();
            PlayerEvents.StressInactiveEvent();
        }

        #endregion
    }
}