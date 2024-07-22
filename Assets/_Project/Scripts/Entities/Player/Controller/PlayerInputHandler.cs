using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ATBMI.Enum;
using UnityEngine.Windows;

namespace ATBMI.Entities.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Fields & Property

        [Header("Action Map Name Reference")]
        [SerializeField] private string playerMapName = "Player";
        [SerializeField] private string uiMapName = "UI";

        [Header("Action Map Name Reference")]
        [SerializeField] private string move = "Move";
        [SerializeField] private string interact = "Interact";
        [SerializeField] private string select = "Select";
        [SerializeField] private string navigate = "Navigate";

        // Input Action References
        private InputActionAsset _playerActions;
        private InputAction _moveAction;
        private InputAction _interactAction;
        private InputAction _selectAction;
        private InputAction _navigateAction;
        
        // Action Values
        public Vector2 MoveDirection { get; private set; }
        public bool InteractTriggered { get; private set; }
        public bool SelectTriggered { get; private set; }
        public bool NavigateUp { get; private set; }
        public bool NavigateDown { get; private set; }
        
        #endregion
        
        #region MonoBehaviour Callbacks
        
        private void Awake()
        {   
            _playerActions = GetComponent<PlayerInput>().actions;

            _moveAction = _playerActions.FindActionMap(playerMapName).FindAction(move);
            _interactAction = _playerActions.FindActionMap(playerMapName).FindAction(interact);
            _selectAction = _playerActions.FindActionMap(playerMapName).FindAction(select);
            _navigateAction = _playerActions.FindActionMap(uiMapName).FindAction(navigate);
        }

        private void OnEnable()
        {
            _moveAction.Enable();
            _moveAction.performed += value => MoveDirection = value.ReadValue<Vector2>();
            _moveAction.canceled += value => MoveDirection = Vector2.zero;

            _interactAction.Enable();
            _interactAction.started += value => InteractTriggered = true;
            _interactAction.canceled += value => InteractTriggered = false;

            _selectAction.Enable();
            _selectAction.started += value => SelectTriggered = true;
            _selectAction.canceled += value => SelectTriggered = false;

            _navigateAction.Enable();
            _navigateAction.performed += value =>
                {
                    var navigateValue = value.ReadValue<Vector2>();
                    NavigateUp = navigateValue.x > 0;
                    NavigateDown = navigateValue.x < 0;
                };
            _navigateAction.canceled += value =>
                {
                    NavigateUp = false;
                    NavigateDown = false;
                };
        }
        
        private void OnDisable()
        {
            _moveAction.Disable();
            _moveAction.performed -= value => MoveDirection = value.ReadValue<Vector2>();
            _moveAction.canceled -= value => MoveDirection = Vector2.zero;

            _interactAction.Disable();
            _interactAction.started -= value => InteractTriggered = true;
            _interactAction.canceled -= value => InteractTriggered = false;

            _selectAction.Disable();
            _selectAction.started -= value => SelectTriggered = true;
            _selectAction.canceled -= value => SelectTriggered = false;

            _navigateAction.Disable();
            _navigateAction.performed -= value =>
                {
                    var navigateValue = value.ReadValue<Vector2>();
                    NavigateUp = navigateValue.x > 0;
                    NavigateDown = navigateValue.x < 0;
                };
            _navigateAction.canceled -= value =>
                {
                    NavigateUp = false;
                    NavigateDown = false;
                };
        }

        #endregion

        #region Methods

        public bool IsPressInteract()
        {
            return _interactAction.WasPressedThisFrame();
        }
        
        public bool IsPressSelect()
        {
            return _selectAction.WasPressedThisFrame();
        }

        public bool IsPressNavigate(NavigateState state)
        {
            var value = state switch
            {
                NavigateState.Up => NavigateUp,
                NavigateState.Down => NavigateDown,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
            };

            return _navigateAction.WasPressedThisFrame() && value;
        }
        
        #endregion
    }
}
