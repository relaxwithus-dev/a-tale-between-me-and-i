using UnityEngine;

namespace ATBMI.Interaction
{
    public interface IInteractable
    {
        public bool Validate() { return true; }
        public Transform GetSignTransform();
        public void Interact(int itemId = 0);
    }
}