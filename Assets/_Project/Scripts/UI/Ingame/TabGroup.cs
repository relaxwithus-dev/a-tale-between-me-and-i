using System.Collections.Generic;
using UnityEngine;
using ATBMI.Gameplay.Controller;

namespace ATBMI.UI.Ingame
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private List<GameObject> pages;

        // [SerializeField] private Color tabIdleColor;
        // [SerializeField] private Color tabActiveColor;

        private int _currentTabIndex;
        private TabButton _selectedTab;

        private void Start()
        {
            if (tabButtons.Count < 1)
            {
                Debug.Log("tab buttons is empty!");
                return;
            }
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

        // private void ResetTabs()
        // {
        //     foreach (var tab in tabButtons)
        //     {
        //         tab.background.color = tabIdleColor;
        //         // tab.selectedIcon.SetActive(false);
        //     }
        // }

        private void SelectTab(int index)
        {
            if (_selectedTab != null)
                _selectedTab.Deselect();

            _selectedTab = tabButtons[index];
            _selectedTab.Select();

            // ResetTabs();
            // _selectedTab.background.color = tabActiveColor;
            // _selectedTab.selectedIcon.SetActive(true);
            for (var i = 0; i < pages.Count; i++)
            {
                pages[i].SetActive(i == index);
            }

            Debug.Log(_selectedTab.gameObject.name);
        }

        public void SelectTabByName(UIMenuTabEnum tab)
        {
            _currentTabIndex = (int)tab;

            switch (tab)
            {
                case UIMenuTabEnum.Quest:
                    SelectTab((int)UIMenuTabEnum.Quest);
                    break;
                case UIMenuTabEnum.Inventory:
                    SelectTab((int)UIMenuTabEnum.Inventory);
                    break;
                case UIMenuTabEnum.Map:
                    SelectTab((int)UIMenuTabEnum.Map);
                    break;
                case UIMenuTabEnum.Setting:
                    SelectTab((int)UIMenuTabEnum.Setting);
                    break;
                default:
                    break;
            }
        }
    }
}
