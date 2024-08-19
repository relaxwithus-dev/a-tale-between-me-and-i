using System;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public class PlayerEventHandler
    {
        // Event
        public static event Action OnInteract;
        public static event Action OnStressActive;
        public static event Action OnStressInactive;
        public static event Action<TextAsset, float, bool> OnMoveToPlayer;

        // Caller
        public static void InteractEvent() => OnInteract?.Invoke();
        public static void StressActiveEvent() => OnStressActive?.Invoke();
        public static void StressInactiveEvent() => OnStressInactive?.Invoke();
        public static void MoveToPlayerEvent(TextAsset INKJson, float newPositionX, bool isNpcFacingRight) => OnMoveToPlayer?.Invoke(INKJson, newPositionX, isNpcFacingRight);
    }
}