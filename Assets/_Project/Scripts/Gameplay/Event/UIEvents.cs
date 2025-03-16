using System;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public static class UIEvents
    {
        public static event Action OnSelectTabInventory;
        public static event Action OnDeselectTabInventory;

        public static void OnSelectTabInventoryEvent() => OnSelectTabInventory?.Invoke();
        public static void OnDeselectTabInventoryEvent() => OnDeselectTabInventory?.Invoke();
    }
}
