using ATBMI.Cutscene;
using ATBMI.Dialogue;
using UnityEngine;
using ATBMI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Handler;

namespace ATBMI.UI.Ingame
{
    public class UIMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject menuUI;
        [SerializeField] private TabGroup tabGroup;
        
        private bool _isMenuActive;

        public bool IsMenuActive => _isMenuActive;

        [Header("Reference")]
        [SerializeField] private PlayerController playerController;
        
        private void Update()
        {
            if (DialogueManager.Instance.IsDialoguePlaying || CutsceneManager.Instance.IsCutscenePlaying) return;
            
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
