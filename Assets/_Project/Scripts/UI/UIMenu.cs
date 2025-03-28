using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI
{
    public class UIMenu : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject menuUI;

        private bool isMenuActive;

        public bool IsMenuActive => isMenuActive;

        private void Awake()
        {
            isMenuActive = false;
        }

        private void Update()
        {
            // TODO: change with new input system
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isMenuActive)
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
            isMenuActive = true;
        }

        private void CloseMenu()
        {
            UIEvents.OnDeselectTabInventoryEvent();
            UIEvents.OnDeselectTabQuestEvent();

            menuUI.SetActive(false);
            isMenuActive = false;
        }
    }
}
