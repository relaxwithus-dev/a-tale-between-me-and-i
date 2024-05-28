using System;
using System.Collections.Generic;

namespace ATBMI.Stress
{
    public class StressEventHandler
    {
        public static event Action OnStressOvertime;
        public static event Action<bool> OnStressOnce;

        public static void StressOvertimeEvent() => OnStressOvertime?.Invoke();
        public static void StressOnceEvent(bool condition) => OnStressOnce?.Invoke(condition);
    }
}