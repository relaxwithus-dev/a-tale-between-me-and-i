using System;
using System.Collections.Generic;

namespace ATMBI.Gameplay.EventHandler
{
    public class PlayerEventHandler
    {
        public delegate void Stress();
        public static event Stress OnStress;

        public static void CallStressEvent() => OnStress?.Invoke();
    }
}