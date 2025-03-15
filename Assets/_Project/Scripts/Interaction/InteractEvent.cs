using System;

namespace ATBMI.Interaction
{
    public static class InteractEvent
    {
        // Event
        public static event Action<bool> OnInteracted;
        public static event Action<bool> OnRestricted;
        
        // Caller
        public static void InteractedEvent(bool interact) => OnInteracted?.Invoke(interact);
        public static void RestrictedEvent(bool restrict) => OnRestricted?.Invoke(restrict);
    }
}