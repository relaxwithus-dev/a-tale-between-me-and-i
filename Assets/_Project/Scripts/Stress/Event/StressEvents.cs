using System;

namespace ATBMI.Stress
{
    public class StressEvents
    {
        public static event Action<bool> OnStressOvertime;
        public static event Action<bool, float> OnStressOnce;
        
        public static void StressOvertimeEvent(bool isAddStress) => OnStressOvertime?.Invoke(isAddStress);
        public static void StressOnceEvent(bool isAddStress, float value) => OnStressOnce?.Invoke(isAddStress, value);
    }
}