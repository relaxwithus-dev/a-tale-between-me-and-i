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
        [SerializeField] [Range(0.5f, 4f)] private float increasedGapTime = 2f;
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
            _increasedValue = maxStressValue * (2f/100f);
        }

        #endregion

        #region Methods

        private void HandleStressOvertime()
        {
            StartCoroutine(StressOvertimeRoutine());
        }

        private IEnumerator StressOvertimeRoutine()
        {
            yield return new WaitForSeconds(increasedGapTime);

            CurrentStressValue += _increasedValue;
            yield return StressSliderRoutine();
            if (CurrentStressValue >= maxStressValue)
            {
                CurrentStressValue = maxStressValue;
                PlayerEventHandler.StressEvent();
            }
        }

        private IEnumerator StressSliderRoutine()
        {
            var value = MAX_SLIDER_VALUE - (CurrentStressValue / maxStressValue);
            while (sliderUI.value < value)
            {
                sliderUI.value += 7 * Time.deltaTime;
                yield return null;
            }
            sliderUI.value = value;
        }

        #endregion
    }
}