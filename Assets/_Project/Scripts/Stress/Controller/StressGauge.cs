using System;
using System.Collections;
using System.Collections.Generic;
using ATMBI.Gameplay.EventHandler;
using UnityEngine;
using UnityEngine.UI;

namespace ATBMI.Stress
{
    public class StressGauge : MonoBehaviour
    {
        #region Fields & Property

        [Header("Gauge")]
        [SerializeField] private Slider sliderUI;
        [SerializeField] private float maxStressValue;
        [SerializeField] [Range(0.5f, 5f)] private float increasedGapTime = 1f;
        [SerializeField] private float statusDuration = 30f;
        
        private float _increasedValue;

        private float CurrentStressValue { get; set; }
        private const float MAX_SLIDER_VALUE = 1f;
        private const float MIN_SLIDER_VALUE = 0f;

        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            StressEventHandler.OnStressOvertime += HandleStressOvertime;
        }

        private void OnDisable()
        {
            StressEventHandler.OnStressOvertime -= HandleStressOvertime;
        }

        private void Start()
        {
            sliderUI.value = MIN_SLIDER_VALUE;
            CurrentStressValue = sliderUI.value;
            _increasedValue = CalculatePercentage(maxStressValue, 2f);
        }

        #endregion

        #region Methods

        private void HandleStressOvertime()
        {
            StartCoroutine(StressOvertimeRoutine());
        }

        private IEnumerator StressOvertimeRoutine()
        {
            while (CurrentStressValue < maxStressValue)
            {
                yield return new WaitForSeconds(increasedGapTime);
                CurrentStressValue += _increasedValue;
                yield return IncreaseSliderRoutine();

                if (CurrentStressValue >= maxStressValue)
                {
                    CurrentStressValue = maxStressValue;
                    sliderUI.value = MAX_SLIDER_VALUE;
                    
                    PlayerEventHandler.StressActiveEvent();
                    yield return DecreaseSliderRoutine();
                }
                yield return null;
            }
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
            while (elapsedTime < statusDuration)
            {
                elapsedTime += Time.deltaTime;
                sliderUI.value = Mathf.Lerp(MAX_SLIDER_VALUE, MIN_SLIDER_VALUE, elapsedTime/statusDuration);
                yield return null;
            }

            sliderUI.value = MIN_SLIDER_VALUE;
            PlayerEventHandler.StressInactiveEvent();
        }

        private float CalculatePercentage(float value, float percentage)
        {
            return value * (percentage / 100f);
        }

        #endregion
    }
}