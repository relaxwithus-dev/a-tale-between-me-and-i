using UnityEngine;
using UnityEngine.UI;
using ATBMI.Gameplay.Handler;

namespace ATBMI.UI.Menu
{
    public class ButtonNavigationHandler : MonoBehaviour
    {
        [SerializeField] private Button[] buttons;
        
        private int _currentIndex;
		private int _arrowChildIndex;
        private Button _selectedButton;
        private readonly int MinIndex = 0;
        
        private void Start()
        {
			// Initiate
			_currentIndex = 0;
			_arrowChildIndex = 0;

            // Setup arrow indicator
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
        
        private void Update()
        {
            // Handle navigation
            if (GameInputHandler.Instance.IsArrowDown)
                _currentIndex++;
            else if (GameInputHandler.Instance.IsArrowUp)
                _currentIndex--;
            else if (GameInputHandler.Instance.IsTapInteract)
                _selectedButton.onClick.Invoke();
            
            _currentIndex = Mathf.Clamp(_currentIndex, MinIndex, buttons.Length - 1);
            if (_selectedButton != buttons[_currentIndex])
            {
                _selectedButton.transform.GetChild(_arrowChildIndex).gameObject.SetActive(false);
                _selectedButton = buttons[_currentIndex];
                _selectedButton.transform.GetChild(_arrowChildIndex).gameObject.SetActive(true);
            }
        }
    }
}