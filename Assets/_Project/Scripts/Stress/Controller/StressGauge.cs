using System;
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
        [SerializeField] private float slideDuration;
        [SerializeField] private float increasedPercent;
        [SerializeField] [Range(0.1f, 1f)] private float increasedInterval;

        private float _currentStressValue;
        private float _increasedValue;
        private bool _isStatusActive;
        private Coroutine _overtimeRoutine;
        
        private const float MAX_SLIDER_VALUE = 1f;
        private const float MIN_SLIDER_VALUE = 0f;

        [Header("UI")]
        [SerializeField] private Slider sliderUI;

        #endregion

        #region MonoBehaviour Callbacks

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
            InitStressGauge();
            _increasedValue = stressValue * (increasedPercent / 100f);
        }

        private void Update()
        {
            if (_isStatusActive) return;

            var currentValue = _currentStressValue / stressValue;
            sliderUI.value = Mathf.Lerp(currentValue, MAX_SLIDER_VALUE, slideDuration);
            if (sliderUI.value > MAX_SLIDER_VALUE)
            {
                sliderUI.value = MAX_SLIDER_VALUE;
                StartCoroutine(DecreaseSliderRoutine());
            }
        }

        #endregion

        #region Methods

        // !- Initialize
        private void InitStressGauge()
        {
            _isStatusActive = false;
            _currentStressValue = MIN_SLIDER_VALUE;
            sliderUI.value = MIN_SLIDER_VALUE;
        }
        
        // !- Core
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
            _isStatusActive = true;
            PlayerEvents.StressActiveEvent();

            while (elapsedTime < statusDuration)
            {
                var time = elapsedTime / statusDuration;
                sliderUI.value = Mathf.Lerp(MAX_SLIDER_VALUE, MIN_SLIDER_VALUE, time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            InitStressGauge();
            PlayerEvents.StressInactiveEvent();
        }

        #endregion
    }
}