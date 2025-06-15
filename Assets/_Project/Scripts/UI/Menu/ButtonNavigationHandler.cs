using UnityEngine;
using UnityEngine.UI;
using ATBMI.Audio;
using ATBMI.Gameplay.Controller;

namespace ATBMI.UI.Menu
{
    public class ButtonNavigationHandler : MonoBehaviour
    {
        #region Fields & Properties

        [SerializeField] private bool isKeepArrow;
        [SerializeField] private Button[] buttons;
        
        private int _currentIndex;
		private int _arrowChildIndex;
        private bool _isInitAlready;
        private Button _selectedButton;
        
        private readonly int MinIndex = 0;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void OnEnable()
        {
            if (isKeepArrow && _isInitAlready) return;
            
            _currentIndex = 0;
            _arrowChildIndex = 0;
            _isInitAlready = true;
            
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
                ModifyButtonIndex(isArrowDown: true);
            else if (GameInputHandler.Instance.IsArrowUp)
                ModifyButtonIndex(isArrowDown: false);
            else if (GameInputHandler.Instance.IsTapSelect)
                OnSelectButton();
            
            _currentIndex = Mathf.Clamp(_currentIndex, MinIndex, buttons.Length - 1);
        }
        
        private void ModifyArrowIndicator()
        {
            if (_selectedButton == buttons[_currentIndex]) return;
            
            _selectedButton.transform.GetChild(_arrowChildIndex).gameObject.SetActive(false);
            _selectedButton = buttons[_currentIndex];
            _selectedButton.transform.GetChild(_arrowChildIndex).gameObject.SetActive(true);
        }

        private void ModifyButtonIndex(bool isArrowDown)
        {
            AudioManager.Instance.PlayAudio(Musics.SFX_ChangeUI);
            _currentIndex +=  isArrowDown ? 1 : -1;
        }
        
        private void OnSelectButton()
        {
            AudioManager.Instance.PlayAudio(Musics.SFX_Button);
            _selectedButton.onClick.Invoke();
        }
        
        #endregion
    }
}