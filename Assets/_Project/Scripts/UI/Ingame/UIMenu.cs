using UnityEngine;
using ATBMI.Gameplay.Event;
using ATBMI.Gameplay.Handler;

namespace ATBMI.UI.Ingame
{
    public class UIMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject menuUI;
        [SerializeField] private TabGroup tabGroup;
        private bool _isMenuActive;

        private void Update()
        {
            if (GameInputHandler.Instance.IsOpenQuest && !_isMenuActive)
                OpenMenu(UIMenuTabEnum.Quest);
            else if (GameInputHandler.Instance.IsOpenInventory && !_isMenuActive)
                OpenMenu(UIMenuTabEnum.Inventory);
            else if (GameInputHandler.Instance.IsOpenMap && !_isMenuActive)
                OpenMenu(UIMenuTabEnum.Map);
            else if (GameInputHandler.Instance.IsOpenSetting && !_isMenuActive)
                OpenMenu(UIMenuTabEnum.Setting);
            else if (GameInputHandler.Instance.IsTapBack && _isMenuActive)
                CloseMenu();
        }

        //TODO: Change it to dynamically call when input for inventory pressed
        public void OpenMenu(UIMenuTabEnum tab)
        {
            UIEvents.OnSelectTabInventoryEvent();
            UIEvents.OnSelectTabQuestEvent();

            UIEvents.OnSelectTabSettingEvent();

            menuUI.SetActive(true);
            _isMenuActive = true;

            tabGroup.SelectTabByName(tab);
        }

        public void CloseMenu()
        {
            UIEvents.OnDeselectTabInventoryEvent();
            UIEvents.OnDeselectTabQuestEvent();

            UIEvents.OnDeselectTabSettingEvent();

            menuUI.SetActive(false);
            _isMenuActive = false;
        }
    }
}
