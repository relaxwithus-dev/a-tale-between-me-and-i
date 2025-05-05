using UnityEngine;
using UnityEngine.UI;
using ATBMI.Gameplay.Event;

namespace ATBMI.UI.Ingame
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour
    {
        public UIMenuTabEnum uIMenuTab;
        public Image background;

        //TODO: Add some method when this tab is selected
        public void Select()
        {
            QuestEvents.CheckUIMenuTabQuestStepEvent(uIMenuTab);
            
            switch (uIMenuTab)
            {
                case UIMenuTabEnum.Inventory:
                    UIEvents.OnSelectTabInventoryEvent();
                    break;
                case UIMenuTabEnum.Quest:
                    UIEvents.OnSelectTabQuestEvent();
                    break;
            }
        }

        //TODO: Add some method when this tab is deselected
        public void Deselect()
        {
            switch (uIMenuTab)
            {
                case UIMenuTabEnum.Inventory:
                    UIEvents.OnDeselectTabInventoryEvent();
                    break;
                case UIMenuTabEnum.Quest:
                    UIEvents.OnDeselectTabQuestEvent();
                    break;
            }
        }
    }
}
