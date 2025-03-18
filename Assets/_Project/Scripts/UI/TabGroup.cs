using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBMI
{
    public class TabGroup : MonoBehaviour
    {

        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private List<GameObject> pages;

        [SerializeField] private Color tabIdleColor;
        [SerializeField] private Color tabActiveColor;
        [SerializeField] private InputActionReference navigateLeftAction;
        [SerializeField] private InputActionReference navigateRightAction;

        private TabButton selectedTab;
        private int currentTabIndex = 0;

        private void OnEnable()
        {
            navigateLeftAction.action.Enable();
            navigateRightAction.action.Enable();

            navigateLeftAction.action.performed += MoveToPreviousTab;
            navigateRightAction.action.performed += MoveToNextTab;
        }

        private void OnDisable()
        {
            navigateLeftAction.action.performed -= MoveToPreviousTab;
            navigateRightAction.action.performed -= MoveToNextTab;

            navigateLeftAction.action.Disable();
            navigateRightAction.action.Disable();
        }

        private void Start()
        {
            if (tabButtons.Count > 0)
            {
                currentTabIndex = 0;
                SelectTab(currentTabIndex); // Start with the first tab selected
            }
        }

        private void MoveToNextTab(InputAction.CallbackContext context)
        {
            if (currentTabIndex < tabButtons.Count - 1) // Stop at the last tab
            {
                currentTabIndex++;
                SelectTab(currentTabIndex);
            }
        }

        private void MoveToPreviousTab(InputAction.CallbackContext context)
        {
            if (currentTabIndex > 0) // Stop at the first tab
            {
                currentTabIndex--;
                SelectTab(currentTabIndex);
            }
        }

        private void SelectTab(int index)
        {
            if (selectedTab != null)
            {
                selectedTab.Deselect();
            }

            selectedTab = tabButtons[index];
            selectedTab.Select();

            ResetTabs();
            selectedTab.background.color = tabActiveColor;

            // Activate the correct page based on the tab index
            for (int i = 0; i < pages.Count; i++)
            {
                pages[i].SetActive(i == index);
            }
        }

        private void ResetTabs()
        {
            foreach (var tab in tabButtons)
            {
                tab.background.color = tabIdleColor;
            }
        }
    }
}
