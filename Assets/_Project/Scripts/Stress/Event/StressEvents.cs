using System;

namespace ATBMI.Stress
{
    public class StressEvents
    {
        // Event
        public static event Action<bool> OnStressOvertime;
        public static event Action<bool, float> OnStressOnce;
        
        // Caller
        public static void StressOvertimeEvent(bool isAddStress) => OnStressOvertime?.Invoke(isAddStress);
        public static void StressOnceEvent(bool isAddStress, float value) => OnStressOnce?.Invoke(isAddStress, value);
    }
}