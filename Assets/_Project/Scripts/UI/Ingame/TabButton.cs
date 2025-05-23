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
        public Animator selectedIconAnim;

        private const string IDLE = "A_SelectIdle";
        private const string SELECT = "A_SelectedTab";
        private const string DESELECT = "A_DeselectedTab";

        private void Awake()
        {
            selectedIconAnim.Play(IDLE);
            selectedIconAnim.gameObject.SetActive(true);
        }

        //TODO: Add some method when this tab is selected
        public void Select()
        {
            switch (uIMenuTab)
            {
                case UIMenuTabEnum.Inventory:
                    UIEvents.OnSelectTabInventoryEvent();
                    // selectedIconAnim.Play(SELECT);
                    break;
                case UIMenuTabEnum.Quest:
                    UIEvents.OnSelectTabQuestEvent();
                    // selectedIconAnim.Play(SELECT);
                    break;
                case UIMenuTabEnum.Map:
                    // selectedIconAnim.Play(SELECT);
                    break;
                case UIMenuTabEnum.Setting:
                    // selectedIconAnim.Play(SELECT);
                    break;
            }

            selectedIconAnim.Play(SELECT);
            QuestEvents.CheckUIMenuTabQuestStepEvent(uIMenuTab);
        }

        //TODO: Add some method when this tab is deselected
        public void Deselect()
        {
            switch (uIMenuTab)
            {
                case UIMenuTabEnum.Inventory:
                    UIEvents.OnDeselectTabInventoryEvent();
                    // selectedIconAnim.Play(DESELECT);
                    break;
                case UIMenuTabEnum.Quest:
                    UIEvents.OnDeselectTabQuestEvent();
                    // selectedIconAnim.Play(DESELECT);
                    break;
                case UIMenuTabEnum.Map:
                    // selectedIconAnim.Play(DESELECT);
                    break;
                case UIMenuTabEnum.Setting:
                    // selectedIconAnim.Play(DESELECT);
                    break;
            }

            selectedIconAnim.Play(DESELECT);
        }
    }
}
