using System;
using UnityEngine;

namespace ATBMI.Entities
{
    public class AnimateEventReceiver : MonoBehaviour
    {
        public event Action OnStepDown;
        public void StepDownEvent() => OnStepDown?.Invoke();
    }
}