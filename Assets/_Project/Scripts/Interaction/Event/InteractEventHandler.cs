using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class InteractEventHandler
    {
        public delegate void OpenInteract();
        public static event OpenInteract OnOpenInteract;

        public delegate void CloseInteract();
        public static event OpenInteract OnCloseInteract;

        public static void OpenInteractEvent() => OnOpenInteract?.Invoke();
        public static void CloseInteractEvent() => OnCloseInteract?.Invoke();


    }
}