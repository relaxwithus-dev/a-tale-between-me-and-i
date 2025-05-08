using UnityEngine;
using ATBMI.Gameplay.Event;
using ATBMI.Gameplay.Handler;

namespace ATBMI.UI.Ingame
{
    public class UIMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject menuUI;
        private bool _isMenuActive;

        private void Update()
        {
            if (GameInputHandler.Instance.IsTapPhone && !_isMenuActive)
                OpenMenu();
            else if (GameInputHandler.Instance.IsTapBack && _isMenuActive)
                CloseMenu();
        }

        //TODO: Change it to dynamically call when input for inventory pressed
        private void OpenMenu()
        {
            UIEvents.OnSelectTabInventoryEvent();
            UIEvents.OnSelectTabQuestEvent();

            menuUI.SetActive(true);
            _isMenuActive = true;
        }
        
        private void CloseMenu()
        {
            UIEvents.OnDeselectTabInventoryEvent();
            UIEvents.OnDeselectTabQuestEvent();

            menuUI.SetActive(false);
            _isMenuActive = false;
        }
    }
}
