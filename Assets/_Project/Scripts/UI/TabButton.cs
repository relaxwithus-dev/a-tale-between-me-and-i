using System;
using ATBMI.Gameplay.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ATBMI
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour
    {
        public UIMenuTabEnum uIMenuTab;
        public Image background;

        public void Select()
        {
            //TODO: add some method when this tab is selected
            switch (uIMenuTab)
            {
                case UIMenuTabEnum.Inventory:
                    UIEvents.OnSelectTabInventoryEvent();
                    break;
                default:
                    break;
            }
        }

        public void Deselect()
        {
            //TODO: add some method when this tab is deselected
            switch (uIMenuTab)
            {
                case UIMenuTabEnum.Inventory:
                    UIEvents.OnDeselectTabInventoryEvent();
                    break;
                default:
                    break;
            }
        }
    }
}
