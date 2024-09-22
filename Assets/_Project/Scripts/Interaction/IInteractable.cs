namespace ATBMI.Interaction
{
    public interface IInteractable
    {
        public void Interact(InteractManager manager, int itemId = 0);
        public bool Status();
    }
}