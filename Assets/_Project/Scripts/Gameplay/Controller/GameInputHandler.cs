using UnityEngine;
using UnityEngine.InputSystem;
using ATBMI.Singleton;

namespace ATBMI.Gameplay.Controller
{
    /// <summary>
    /// GameInputHandler buat handle semua input mapping,
    /// mulai dari player sampe UI.
    /// </summary>
    public class GameInputHandler : MonoSingleton<GameInputHandler>
    {
        #region Fields & Property

        [Header("Action Maps Reference")]
        [SerializeField] private string playerMapName = "Player";
        [SerializeField] private string uiMapName = "UI";

        [Header("Player Actions Reference")]
        [SerializeField] private string move = "Move";
        [SerializeField] private string run = "Run";
        [SerializeField] private string interact = "Interact";
        [SerializeField] private string phone = "Phone";
        [SerializeField] private string setting = "Setting";

        // Input action
        private InputActionAsset _playerActions;
        private InputAction _moveAction;
        private InputAction _runAction;
        private InputAction _interactAction;
        private InputAction _phoneAction;
        private InputAction _settingAction;
        
        // Action values
        public Vector2 MoveDirection { get; private set; }
        public bool IsPressRun { get; private set; }
        public bool IsTapInteract => _interactAction.WasPressedThisFrame();
        public bool IsOpenPhone => _phoneAction.WasPressedThisFrame();
        public bool IsOpenSetting => _settingAction.WasPressedThisFrame();
        public bool IsClosePhone => _phoneAction.WasPressedThisFrame() || _settingAction.WasPressedThisFrame();

        [Header("UI Actions Reference")]
        [SerializeField] private string navigate = "Navigate";
        [SerializeField] private string select = "Select";
        [SerializeField] private string close = "Close";
        [SerializeField] private string tabMenu = "TabMenu";
        
        // Input action
        private InputAction _navigateAction;
        private InputAction _selectAction;
        private InputAction _closeAction;
        private InputAction _tabMenuAction;
        
        // Action values
        private bool _isArrowUp;
        private bool _isArrowDown;
        private bool _isArrowLeft;
        private bool _isArrowRight;
        
        private bool _isTabLeft;
        private bool _isTabRight;
        
        public bool IsArrowUp => _isArrowUp && _navigateAction.WasPressedThisFrame();
        public bool IsArrowDown => _isArrowDown && _navigateAction.WasPressedThisFrame();
        public bool IsArrowLeft => _isArrowLeft && _navigateAction.WasPressedThisFrame();
        public bool IsArrowRight => _isArrowRight && _navigateAction.WasPressedThisFrame();
        
        public bool IsTapSelect => _selectAction.WasPressedThisFrame();
        public bool IsTapClose => _closeAction.WasPressedThisFrame();
        
        public bool IsTabRight => _isTabRight && _tabMenuAction.WasPressedThisFrame();
        public bool IsTabLeft => _isTabLeft && _tabMenuAction.WasPressedThisFrame();
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        protected override void Awake()
        {
            _playerActions = GetComponent<PlayerInput>().actions;

            // Player
            _moveAction = _playerActions.FindActionMap(playerMapName).FindAction(move);
            _runAction = _playerActions.FindActionMap(playerMapName).FindAction(run);
            _interactAction = _playerActions.FindActionMap(playerMapName).FindAction(interact);
            _phoneAction = _playerActions.FindActionMap(playerMapName).FindAction(phone);
            _settingAction = _playerActions.FindActionMap(playerMapName).FindAction(setting);
            
            // UI
            _navigateAction = _playerActions.FindActionMap(uiMapName).FindAction(navigate);
            _selectAction = _playerActions.FindActionMap(uiMapName).FindAction(select);
            _closeAction = _playerActions.FindActionMap(uiMapName).FindAction(close);
            _tabMenuAction = _playerActions.FindActionMap(uiMapName).FindAction(tabMenu);
        }

        private void OnEnable()
        {
            SubscribeAction();
        }
        
        private void OnDisable()
        {
            UnsubscribeAction();
        }

        // Initialize
        private void SubscribeAction()
        {
            // Player
            _moveAction.Enable();
            _moveAction.performed += OnMovePerformed;
            _moveAction.canceled += OnMoveCanceled;
            
            _runAction.Enable();
            _runAction.performed += OnRunPerformed;
            _runAction.canceled += OnRunCanceled;
            
            _interactAction.Enable();
            _phoneAction.Enable();
            _settingAction.Enable();
            
            // UI
            _navigateAction.Enable();
            _navigateAction.performed += OnNavigationPerformed;
            _navigateAction.canceled += OnNavigationCanceled;
            
            _selectAction.Enable();
            _closeAction.Enable();
            
            _tabMenuAction.Enable();
            _tabMenuAction.performed += OnMenuPerformed;
            _tabMenuAction.canceled += OnMenuCanceled;
        }
        
        private void UnsubscribeAction()
        {
            // Player
            _moveAction.Disable();
            _moveAction.performed -= OnMovePerformed;
            _moveAction.canceled -= OnMoveCanceled;
            
            _runAction.Disable();
            _runAction.performed -= OnRunPerformed;
            _runAction.canceled -= OnRunCanceled;
            
            _interactAction.Disable();
            _phoneAction.Disable();
            _settingAction.Enable();
            
            // UI
            _navigateAction.Disable();
            _navigateAction.performed -= OnNavigationPerformed;
            _navigateAction.canceled -= OnNavigationCanceled;
            
            _selectAction.Disable();
            _closeAction.Disable();
            
            _tabMenuAction.Disable();
            _tabMenuAction.performed -= OnMenuPerformed;
            _tabMenuAction.canceled -= OnMenuCanceled;
        }
        
        // Core
        private void OnMovePerformed(InputAction.CallbackContext value) => MoveDirection = value.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext value) => MoveDirection = Vector2.zero;

        private void OnRunPerformed(InputAction.CallbackContext value) => IsPressRun = true;
        private void OnRunCanceled(InputAction.CallbackContext value) => IsPressRun = false;
        
        private void OnNavigationPerformed(InputAction.CallbackContext value)
        {
            var navigateValue = value.ReadValue<Vector2>();
            _isArrowRight = navigateValue.x > 0;
            _isArrowLeft = navigateValue.x < 0;
                    
            _isArrowUp = navigateValue.y > 0;
            _isArrowDown = navigateValue.y < 0;
        }
        
        private void OnNavigationCanceled(InputAction.CallbackContext value)
        {
            _isArrowRight = false;
            _isArrowLeft = false;
                    
            _isArrowUp = false;
            _isArrowDown = false;
        }
        
        private void OnMenuPerformed(InputAction.CallbackContext value)
        {
            var tabValue = value.ReadValue<Vector2>();
            _isTabRight = tabValue.x > 0;
            _isTabLeft = tabValue.x < 0;
        }
        
        private void OnMenuCanceled(InputAction.CallbackContext value)
        {
            _isTabRight = false;
            _isTabLeft = false;
        }
        
        #endregion
    }
}
