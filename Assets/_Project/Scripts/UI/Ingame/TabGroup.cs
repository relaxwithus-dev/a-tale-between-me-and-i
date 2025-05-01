using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBMI.UI.Ingame
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private List<GameObject> pages;
        
        [SerializeField] private Color tabIdleColor;
        [SerializeField] private Color tabActiveColor;
        [SerializeField] private InputActionReference navigateLeftAction;
        [SerializeField] private InputActionReference navigateRightAction;
        
        private TabButton _selectedTab;
        private int _currentTabIndex;
        
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
                _currentTabIndex = 0;
                SelectTab(_currentTabIndex); // Start with the first tab selected
            }
        }

        private void MoveToNextTab(InputAction.CallbackContext context)
        {
            if (_currentTabIndex < tabButtons.Count - 1) // Stop at the last tab
            {
                _currentTabIndex++;
                SelectTab(_currentTabIndex);
            }
        }

        private void MoveToPreviousTab(InputAction.CallbackContext context)
        {
            if (_currentTabIndex > 0) // Stop at the first tab
            {
                _currentTabIndex--;
                SelectTab(_currentTabIndex);
            }
        }

        private void SelectTab(int index)
        {
            if (_selectedTab != null)
            {
                _selectedTab.Deselect();
            }

            _selectedTab = tabButtons[index];
            _selectedTab.Select();

            ResetTabs();
            _selectedTab.background.color = tabActiveColor;

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
