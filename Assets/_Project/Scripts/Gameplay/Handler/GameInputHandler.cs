using UnityEngine;
using UnityEngine.InputSystem;
using ATBMI.Singleton;

namespace ATBMI.Gameplay.Handler
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

        // Input action
        private InputActionAsset _playerActions;
        private InputAction _moveAction;
        private InputAction _runAction;
        private InputAction _interactAction;
        private InputAction _phoneAction;

        // Action values
        public Vector2 MoveDirection { get; private set; }
        public bool IsPressRun { get; private set; }
        public bool IsTapInteract => _interactAction.WasPressedThisFrame();
        public bool IsTapPhone => _phoneAction.WasPressedThisFrame();

        [Header("UI Actions Reference")]
        [SerializeField] private string navigate = "Navigate";
        [SerializeField] private string select = "Select";
        [SerializeField] private string back = "Back";

        // Input action
        private InputAction _navigateAction;
        private InputAction _selectAction;
        private InputAction _backAction;
        
        // Action values
        private bool _isNavigateUp;
        private bool _isNavigateDown;
        
        public bool IsNavigateUp => _isNavigateUp && _navigateAction.WasPressedThisFrame();
        public bool IsNavigateDown => _isNavigateDown && _navigateAction.WasPressedThisFrame();
        public bool IsTapSelect => _selectAction.WasPressedThisFrame();
        public bool IsTapBack => _backAction.WasPressedThisFrame();
    
        #endregion
        
        #region MonoBehaviour Callbacks
        
        protected override void Awake()
        {
            _playerActions = GetComponent<PlayerInput>().actions;

            // Player
            _moveAction = _playerActions.FindActionMap(playerMapName).FindAction(move);
            _runAction = _playerActions.FindActionMap(playerMapName).FindAction(run);
            _interactAction = _playerActions.FindActionMap(playerMapName).FindAction(interact);
            _phoneAction = _playerActions.FindActionMap(playerMapName).FindAction(phone);

            // UI
            _navigateAction = _playerActions.FindActionMap(uiMapName).FindAction(navigate);
            _selectAction = _playerActions.FindActionMap(uiMapName).FindAction(select);
            _backAction = _playerActions.FindActionMap(uiMapName).FindAction(back);
        }

        private void OnEnable()
        {
            SubscribeAction();
        }
        
        private void OnDisable()
        {
            UnsubscribeAction();
        }

        #endregion

        #region Methods

        // !- Initialize
        private void SubscribeAction()
        {
            // Movement
            _moveAction.Enable();
            _moveAction.performed += value => MoveDirection = value.ReadValue<Vector2>();
            _moveAction.canceled += value => MoveDirection = Vector2.zero;

            // Run
            _runAction.Enable();
            _runAction.performed += value => IsPressRun = true;
            _runAction.canceled += value => IsPressRun = false;

            // Navigation
            _navigateAction.Enable();
            _navigateAction.performed += value =>
                {
                    var navigateValue = value.ReadValue<Vector2>();
                    _isNavigateUp = navigateValue.x > 0;
                    _isNavigateDown = navigateValue.x < 0;
                };
            _navigateAction.canceled += value =>
                {
                    _isNavigateUp = false;
                    _isNavigateDown = false;
                };
        }

        private void UnsubscribeAction()
        {
            // Movement
            _moveAction.Disable();
            _moveAction.performed -= value => MoveDirection = value.ReadValue<Vector2>();
            _moveAction.canceled -= value => MoveDirection = Vector2.zero;

            // Run
            _runAction.Disable();
            _runAction.performed -= value => IsPressRun = true;
            _runAction.canceled -= value => IsPressRun = false;

            // Navigation
            _navigateAction.Disable();
            _navigateAction.performed -= value =>
                {
                    var navigateValue = value.ReadValue<Vector2>();
                    _isNavigateUp = navigateValue.x > 0;
                    _isNavigateDown = navigateValue.x < 0;
                };
            _navigateAction.canceled -= value =>
                {
                    _isNavigateUp = false;
                    _isNavigateDown = false;
                };
        }

        #endregion
    }
}
