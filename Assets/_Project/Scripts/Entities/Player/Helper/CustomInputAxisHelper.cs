using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBMI.Entities.Player
{
    public class CustomInputAxisHelper
    {
        #region Fields & Property
        private float _currentValue;
        private int _lastDirection;

        private bool inputSnap;
        private bool inputInvert;

        private readonly float inputSensitivity;
        private readonly float inputGravity;
        #endregion

        #region Methods
        public CustomInputAxisHelper(float sensitivity, float gravity, bool snap, bool invert)
        {
            inputSensitivity = sensitivity;
            inputGravity = gravity;

            inputSnap = snap;
            inputInvert = invert;

        }

        public float GetInputAxis(float target)
        {
            var targetValue = target;

            if (inputInvert)
            {
                targetValue *= -1f;
            }
    
            if (Mathf.Approximately(targetValue, 0f)) 
            {
                _currentValue = Mathf.MoveTowards(_currentValue, targetValue, Time.deltaTime * inputGravity);
            } 
            else 
            {
                if (inputSnap)
                {
                    var currentDirection = Mathf.RoundToInt(Mathf.Sign(targetValue));
                    var currentValueCondition = currentDirection != 0 && _lastDirection != 0 && currentDirection != _lastDirection;

                    _currentValue = currentValueCondition ? 
                            0f : Mathf.MoveTowards(_currentValue, targetValue, Time.deltaTime * inputSensitivity);

                    _lastDirection = currentDirection;
                } 
                else 
                {
                    _currentValue = Mathf.MoveTowards(_currentValue, targetValue, Time.deltaTime * inputSensitivity);
                }
            }

            return _currentValue;
        }
        #endregion
    }
}
