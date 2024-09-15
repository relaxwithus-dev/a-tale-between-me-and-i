using System;
using System.Collections.Generic;
using ATBMI;
using UnityEngine;

namespace ATMBI.Gameplay.Event
{
    public class PlayerEventHandler
    {
        // Event
        public static event Action OnInteract;
        public static event Action OnStressActive;
        public static event Action OnStressInactive;
        public static event Action<RuleEntry, TextAsset, float, bool> OnMoveToPlayer;

        // Caller
        public static void InteractEvent() => OnInteract?.Invoke();
        public static void StressActiveEvent() => OnStressActive?.Invoke();
        public static void StressInactiveEvent() => OnStressInactive?.Invoke();
        public static void MoveToPlayerEvent(RuleEntry ruleEntry, TextAsset INKJson, float newPositionX, bool isNpcFacingRight) => OnMoveToPlayer?.Invoke(ruleEntry, INKJson, newPositionX, isNpcFacingRight);
    }
}