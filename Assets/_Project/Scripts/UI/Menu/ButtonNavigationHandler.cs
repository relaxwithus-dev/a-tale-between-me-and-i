using UnityEngine;
using UnityEngine.UI;
using ATBMI.Gameplay.Handler;

namespace ATBMI.UI.Menu
{
    public class ButtonNavigationHandler : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField] private Button[] buttons;
        
        private int _currentIndex;
		private int _arrowChildIndex;
        private Button _selectedButton;
        
        private readonly int MinIndex = 0;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
			_currentIndex = 0;
			_arrowChildIndex = 0;
            
            InitArrowIndicator();
        }
        
        private void Update()
        {
            HandleNavigation();
            ModifyArrowIndicator();
            
        }
        
        // Initialize
        private void InitArrowIndicator()
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var buttonArrow = button.transform.GetChild(_arrowChildIndex).gameObject;
                
                if (i == MinIndex)
                {
                    _selectedButton = button;
                    buttonArrow.SetActive(true);
                    continue;
                }
                buttonArrow.SetActive(false);
            }
        }
        
        // Core
        private void HandleNavigation()
        {
            if (GameInputHandler.Instance.IsArrowDown)
                _currentIndex++;
            else if (GameInputHandler.Instance.IsArrowUp)
                _currentIndex--;
            else if (GameInputHandler.Instance.IsTapSelect)
                _selectedButton.onClick.Invoke();
            
            _currentIndex = Mathf.Clamp(_currentIndex, MinIndex, buttons.Length - 1);
        }
        
        private void ModifyArrowIndicator()
        {
            if (_selectedButton == buttons[_currentIndex]) return;
            
            _selectedButton.transform.GetChild(_arrowChildIndex).gameObject.SetActive(false);
            _selectedButton = buttons[_currentIndex];
            _selectedButton.transform.GetChild(_arrowChildIndex).gameObject.SetActive(true);
        }
        
        #endregion
    }
}