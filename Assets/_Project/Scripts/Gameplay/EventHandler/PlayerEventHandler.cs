using System;
using System.Collections.Generic;

namespace ATMBI.Gameplay.EventHandler
{
    public class PlayerEventHandler
    {
        public static event Action OnInteract;
        public static event Action OnStressActivate;
        public static event Action OnStressInactivate;


        public static void InteractEvent() => OnInteract?.Invoke();
        public static void StressActiveEvent() => OnStressActivate?.Invoke();
        public static void StressInactiveEvent() => OnStressInactivate?.Invoke();
    }
}