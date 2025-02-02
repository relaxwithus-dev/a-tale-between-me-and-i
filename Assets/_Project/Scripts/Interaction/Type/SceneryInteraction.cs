using ATBMI.Enum;
using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class SceneryInteraction : Interaction
    {
        public override void Interact(InteractManager manager, int itemId = 0)
        {
            base.Interact(manager, itemId);

            if (itemId == 0)
            {
                DialogEvents.EnterDialogueEvent();
            }
            else
            {
                // TODO: Apply dialogue, tidak terjadi apa2
                // DialogEvents.EnterDialogueEvent();
            }
        }
    }
}