using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class InteractEventHandler
    {
        public delegate void OpenInteract();
        public event OpenInteract OnOpenInteract;

        public delegate void CloseInteract();
        public event OpenInteract OnCloseInteract;

        public void OpenInteractEvent() => OnOpenInteract?.Invoke();
        public void CloseInteractEvent() => OnCloseInteract?.Invoke();


    }
}