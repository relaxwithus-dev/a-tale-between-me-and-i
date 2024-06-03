using System;
using System.Collections.Generic;

namespace ATBMI.Stress
{
    public class StressEventHandler
    {
        public static event Action<bool> OnStressOvertimeEnter;
        public static event Action<bool> OnStressOnce;
        
        public static void StressOvertimeEvent(bool condition) => OnStressOvertimeEnter?.Invoke(condition);
        public static void StressOnceEvent(bool condition) => OnStressOnce?.Invoke(condition);
    }
}