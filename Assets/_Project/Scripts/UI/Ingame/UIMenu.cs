using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.UI.Ingame
{
    public class UIMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject menuUI;
        private bool _isMenuActive;

        private void Update()
        {
            // TODO: change with new input system
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!_isMenuActive)
                {
                    OpenMenu();
                }
                else
                {
                    CloseMenu();
                }
            }
        }

        private void OpenMenu()
        {
            UIEvents.OnSelectTabInventoryEvent(); //todo: change it to dynamically call when input for inventory pressed
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
