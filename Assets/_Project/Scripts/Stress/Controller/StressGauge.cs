using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ATMBI.Gameplay.Event;

namespace ATBMI.Stress
{
    public class StressGauge : MonoBehaviour
    {
        #region Fields & Property

        [Header("Gauge")]
        [SerializeField] private Slider sliderUI;
        [SerializeField] private float maxStressValue;
        [SerializeField] [Range(0.1f, 5f)] private float increasedInterval = 1f;
        [SerializeField] private float statusDuration = 30f;
        
        private bool _isStatusActive;
        private float _increasedValue;
        private Coroutine _overtimeRoutine;

        private float CurrentStressValue { get; set; }

        private const float MAX_SLIDER_VALUE = 1f;
        private const float MIN_SLIDER_VALUE = 0f;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            StressEventHandler.OnStressOvertimeEnter += HandleStressOvertime;
        }

        private void OnDisable()
        {
            StressEventHandler.OnStressOvertimeEnter -= HandleStressOvertime;
        }

        private void Start()
        {
            InitializeGauge();
            _increasedValue = CalculatePercentage(maxStressValue, 2f);
        }

        #endregion

        #region Methods

        private void InitializeGauge()
        {
            sliderUI.value = MIN_SLIDER_VALUE;
            CurrentStressValue = sliderUI.value;
            _isStatusActive = false;
        }

        private void HandleStressOvertime(bool condition)
        {
            if (condition)
            {
                if (_isStatusActive || _overtimeRoutine != null) return;
                _overtimeRoutine = StartCoroutine(StressOvertimeRoutine());
            }
            else
            {
                if (_isStatusActive || _overtimeRoutine == null) return;
                StopCoroutine(_overtimeRoutine);
                _overtimeRoutine = null;
            }
        }
        
        private IEnumerator StressOvertimeRoutine()
        {
            _isStatusActive = false;
            while (CurrentStressValue < maxStressValue)
            {
                yield return new WaitForSeconds(increasedInterval);
                CurrentStressValue += _increasedValue;
                yield return IncreaseSliderRoutine();

                if (CurrentStressValue >= maxStressValue)
                {
                    CurrentStressValue = maxStressValue;
                    sliderUI.value = MAX_SLIDER_VALUE;
                }
                yield return null;
            }

            yield return DecreaseSliderRoutine();
        }

        private IEnumerator IncreaseSliderRoutine()
        {
            var value = CurrentStressValue / maxStressValue;
            while (sliderUI.value < value)
            {
                sliderUI.value += Time.deltaTime;
                yield return null;
            }

            sliderUI.value = value;
        }

        private IEnumerator DecreaseSliderRoutine()
        {
            var elapsedTime = 0f;
            _isStatusActive = true;
            while (elapsedTime < statusDuration)
            {
                elapsedTime += Time.deltaTime;
                sliderUI.value = Mathf.Lerp(MAX_SLIDER_VALUE, MIN_SLIDER_VALUE, elapsedTime/statusDuration);
                PlayerEventHandler.StressActiveEvent();
                yield return null;
            }

            InitializeGauge();
            PlayerEventHandler.StressInactiveEvent();
        }

        private float CalculatePercentage(float value, float percentage)
        {
            return value * (percentage / 100f);
        }

        #endregion
    }
}