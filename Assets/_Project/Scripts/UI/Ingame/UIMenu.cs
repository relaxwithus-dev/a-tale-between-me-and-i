using UnityEngine;
using ATBMI.Cutscene;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

namespace ATBMI.UI.Ingame
{
    public class UIMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject menuUI;
        [SerializeField] private TabGroup tabGroup;

        private bool _isMenuActive;

        [Header("Reference")]
        [SerializeField] private PlayerController playerController;

        private void Update()
        {
            if (DialogueManager.Instance.IsDialoguePlaying || CutsceneManager.Instance.IsCutscenePlaying) return;

            if (!_isMenuActive)
            {
                if (GameInputHandler.Instance.IsOpenPhone)
                    OpenMenu(UIMenuTabEnum.Quest);
                else if (GameInputHandler.Instance.IsOpenSetting)
                    OpenMenu(UIMenuTabEnum.Setting);
            }
            else
            {
                if (GameInputHandler.Instance.IsOpenPhone || GameInputHandler.Instance.IsOpenSetting)
                    CloseMenu();
            }
        }

        private void OpenMenu(UIMenuTabEnum tab)
        {
            menuUI.SetActive(true);
            playerController.StopMovement();
            tabGroup.SelectTabByName(tab);

            UIEvents.OnSelectTabQuestEvent();
            UIEvents.OnSelectTabSettingEvent();
            UIEvents.OnSelectTabInventoryEvent();

            _isMenuActive = true;
        }

        public void CloseMenu()
        {
            UIEvents.OnDeselectTabInventoryEvent();
            UIEvents.OnDeselectTabQuestEvent();
            UIEvents.OnDeselectTabSettingEvent();

            playerController.StartMovement();
            menuUI.SetActive(false);
            _isMenuActive = false;
        }
    }
}
