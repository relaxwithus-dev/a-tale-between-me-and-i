using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBMI
{
    public class GetAxisScript : MonoBehaviour
    {
        float currentValue;
        int lastDirection;
    
        float sensitivity = 3f, gravity = 3f;
        bool snap = true, invert = false;
    
        InputActionAsset actions;
        string horizontalAxis = "Horizontal";
    
        private void Update() {
            float targetValue = actions[horizontalAxis].ReadValue<float>();
    
            if (invert) {
                targetValue *= -1f;
            }
    
            if (Mathf.Approximately(targetValue, 0f)) {
                currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * gravity);
            } else {
                if (snap) {
                    int currentDirection = Mathf.RoundToInt(Mathf.Sign(targetValue));
    
                    if (currentDirection != 0 && lastDirection != 0 && currentDirection != lastDirection) {
                        currentValue = 0f;
                    } else {
                        currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * sensitivity);
                    }
    
                    lastDirection = currentDirection;
                } else {
                    currentValue = Mathf.MoveTowards(currentValue, targetValue, Time.deltaTime * sensitivity);
                }
                //From here current value should be equal to Input.GetAxis()
            }
        }
    }
}
