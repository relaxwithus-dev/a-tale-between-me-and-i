using ATBMI.Enum;

namespace ATBMI.Interaction
{
    public interface IInteractable
    {
        public void Interact(InteractManager manager, InteractStatus status);
    }
}