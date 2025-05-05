using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Handler;

namespace ATBMI.UI.Ingame
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private List<GameObject> pages;
        
        [SerializeField] private Color tabIdleColor;
        [SerializeField] private Color tabActiveColor;
        
        private int _currentTabIndex;
        private TabButton _selectedTab;

        private void Start()
        {
            if (tabButtons.Count < 1)
            {
                Debug.Log("tab buttons is empty!");
                return;
            }
            
            _currentTabIndex = 0;
            SelectTab(_currentTabIndex);
        }
        
        private void Update()
        {
            if (GameInputHandler.Instance.IsTabLeft)
                MoveToPreviousTab();
            else if (GameInputHandler.Instance.IsTabRight)
                MoveToNextTab();
        }

        private void MoveToNextTab()
        {
            if (_currentTabIndex >= tabButtons.Count - 1) return;
            _currentTabIndex++;
            SelectTab(_currentTabIndex);
        }

        private void MoveToPreviousTab()
        {
            if (_currentTabIndex <= 0) return;
            _currentTabIndex--;
            SelectTab(_currentTabIndex);
        }

        private void SelectTab(int index)
        {
            if (_selectedTab != null)
                _selectedTab.Deselect();

            _selectedTab = tabButtons[index];
            _selectedTab.Select();

            ResetTabs();
            _selectedTab.background.color = tabActiveColor;
            for (var i = 0; i < pages.Count; i++)
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
