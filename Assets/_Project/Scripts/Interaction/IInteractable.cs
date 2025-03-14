using UnityEngine;

namespace ATBMI.Interaction
{
    public interface IInteractable
    {
        public bool Validate() { return true; }
        public Transform GetSignTransform();
        
        public void WhenInteracted(bool interact) {  }
        public void Interact(InteractManager manager, int itemId = 0);
    }
}