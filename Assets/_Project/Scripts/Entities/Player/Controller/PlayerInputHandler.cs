using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBMI.Entities.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Fields & Property

        [Header("Action Map Name Reference")]
        [SerializeField] private string playerMapName = "Player";

        [Header("Action Map Name Reference")]
        [SerializeField] private string move = "Move";
        [SerializeField] private string interact = "Interact";

        // Input Action References
        private InputActionAsset _playerActions;
        private InputAction _moveAction;
        private InputAction _interactAction;

        // Action Values
        public Vector2 MoveDirection { get; private set; }
        public bool InteractTriggered { get; private set; }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {   
            _playerActions = GetComponent<PlayerInput>().actions;

            _moveAction = _playerActions.FindActionMap(playerMapName).FindAction(move);
            _interactAction = _playerActions.FindActionMap(playerMapName).FindAction(interact);
        }

        private void OnEnable()
        {
            _moveAction.Enable();
            _moveAction.performed += value => MoveDirection = value.ReadValue<Vector2>();
            _moveAction.canceled += value => MoveDirection = Vector2.zero;

            _interactAction.Enable();
            _interactAction.performed += value => InteractTriggered = true;
            _interactAction.canceled += value => InteractTriggered = false;
        }
        
        private void OnDisable()
        {
            _moveAction.Disable();
            _moveAction.performed -= value => MoveDirection = value.ReadValue<Vector2>();
            _moveAction.canceled -= value => MoveDirection = Vector2.zero;

            _interactAction.Disable();
            _interactAction.performed -= value => InteractTriggered = true;
            _interactAction.canceled -= value => InteractTriggered = false;
        }

        #endregion

    }
}
