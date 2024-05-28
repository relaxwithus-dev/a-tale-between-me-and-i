using System;
using System.Collections.Generic;

namespace ATMBI.Gameplay.EventHandler
{
    public class PlayerEventHandler
    {
        public static event Action OnInteract;
        public static event Action OnStress;


        public static void InteractEvent() => OnInteract?.Invoke();
        public static void StressEvent() => OnStress?.Invoke();
    }
}