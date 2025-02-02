namespace ATBMI.Interaction
{
    public interface IInteractable
    {
        public bool Validate() { return true; }
        public void Interact(InteractManager manager, int itemId = 0);
    }
}