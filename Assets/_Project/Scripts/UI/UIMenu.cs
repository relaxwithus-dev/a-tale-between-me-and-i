using System.Collections;
using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using UnityEngine;

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

            menuUI.SetActive(true);
            isMenuActive = true;
        }

        private void CloseMenu()
        {
            UIEvents.OnDeselectTabInventoryEvent();

            menuUI.SetActive(false);
            isMenuActive = false;
        }
    }
}
