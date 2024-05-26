using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class InteractEventHandler
    {
        public delegate void OpenInteract();
        public static event OpenInteract OnOpenInteract;

        public static void OpenInteractEvent() => OnOpenInteract?.Invoke();
    }
}