using System;

namespace ATBMI.Gameplay.Event
{
    public static class StressEvents
    {
        // Event
        public static event Action<bool> OnStressOvertime;
        public static event Action<bool, float> OnStressOnce;
        
        // Caller
        public static void StressOvertimeEvent(bool isAddStress) => OnStressOvertime?.Invoke(isAddStress);
        public static void StressOnceEvent(bool isAddStress, float value = 0f) => OnStressOnce?.Invoke(isAddStress, value);
    }
}