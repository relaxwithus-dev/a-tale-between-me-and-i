using System;
using ATBMI.Entities.Player;

namespace ATBMI.Interaction
{
    public static class InteractEvent
    {
        // Event
        public static event Action<bool, PlayerController> OnInteracted;
        public static event Action<bool> OnRestricted;
        
        // Caller
        public static void InteractedEvent(bool interact, PlayerController player) => OnInteracted?.Invoke(interact, player);
        public static void RestrictedEvent(bool restrict) => OnRestricted?.Invoke(restrict);
    }
}