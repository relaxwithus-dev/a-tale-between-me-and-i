using System;
using System.Collections.Generic;
using ATBMI.Entities.Player;
using UnityEngine;
using UnityEngine.UI;

namespace ATBMI.Stress
{
    public class StressGauge : MonoBehaviour
    {
        #region Fields & Property

        [Header("Data")]
        [SerializeField] private Slider stressSlider;
        [SerializeField] private float maxSliderValue;
        [SerializeField] private float stressDuration;
        
        private const float MIN_SLIDER_VALUE = 0;

        #endregion

        #region MonoBehaviour Callbacks

        #endregion

        #region Methods



        #endregion
    }
}