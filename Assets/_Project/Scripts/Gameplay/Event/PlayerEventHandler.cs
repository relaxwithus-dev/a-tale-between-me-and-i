using System;
using System.Collections.Generic;

namespace ATMBI.Gameplay.Event
{
    public class PlayerEventHandler
    {
        // Event
        public static event Action OnInteract;
        public static event Action OnStressActive;
        public static event Action OnStressInactive;

        // Caller
        public static void InteractEvent() => OnInteract?.Invoke();
        public static void StressActiveEvent() => OnStressActive?.Invoke();
        public static void StressInactiveEvent() => OnStressInactive?.Invoke();
    }
}