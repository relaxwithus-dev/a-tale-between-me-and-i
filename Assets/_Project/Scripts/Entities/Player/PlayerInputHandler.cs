using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ATBMI.Entities.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        #region Fields & Property

        public Vector2 Direction { get; private set; }
        private PlayerMoveInput _playerMoveInput;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerMoveInput = new PlayerMoveInput();
        }

        private void OnEnable()
        {
            _playerMoveInput.Enable();
            _playerMoveInput.Player.Move.performed += OnMovementPerformed;
            _playerMoveInput.Player.Move.canceled += OnMovementCanceled;
        }

        private void OnDisable()
        {
            _playerMoveInput.Disable();
            _playerMoveInput.Player.Move.performed -= OnMovementPerformed;
            _playerMoveInput.Player.Move.canceled -= OnMovementCanceled;
        }

        #endregion

        #region Methods

        public void OnMovementPerformed(InputAction.CallbackContext value)
        {
           Direction = value.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext value)
        {
            Direction = Vector2.zero;
        }

        #endregion

    }
}
