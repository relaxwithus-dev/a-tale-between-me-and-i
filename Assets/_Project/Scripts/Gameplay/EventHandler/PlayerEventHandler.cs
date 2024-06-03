using System;
using System.Collections.Generic;

namespace ATMBI.Gameplay.EventHandler
{
    public class PlayerEventHandler
    {
        public static event Action OnInteract;
        public static event Action OnStressActive;
        public static event Action OnStressInactive;


        public static void InteractEvent() => OnInteract?.Invoke();
        public static void StressActiveEvent() => OnStressActive?.Invoke();
        public static void StressInactiveEvent() => OnStressInactive?.Invoke();
    }
}