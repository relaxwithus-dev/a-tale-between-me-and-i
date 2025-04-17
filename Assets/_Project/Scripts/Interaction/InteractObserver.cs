using UnityEngine;

namespace ATBMI.Interaction
{
    public static class InteractObserver
    {
        // Fields
        private static IInteractable _interactable;
        
        // Methods
        public static void Observe(IInteractable interactable) => _interactable = interactable;
        public static IInteractable GetInteractable()
        {
            if (_interactable == null)
            {
                Debug.LogWarning("current interactable is null!");
                return null;
            }
            
            return _interactable;
        }
    }
}