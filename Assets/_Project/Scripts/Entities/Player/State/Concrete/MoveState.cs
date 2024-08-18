using System;
using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class MoveState : PlayerStateBase
    {
        #region Internal Fields
        
        private readonly float _moveSpeed;
        private readonly float _acceleration;
        private readonly float _decceleration;
        private readonly float _velPower;

        private readonly Rigidbody2D _playerRb;

        #endregion
    
        public MoveState(PlayerControllers controller, PlayerDataOld data, PlayerStateSwitcher state, string animationName) : base(controller, data, state, animationName)
        {
            _moveSpeed = data.MoveSpeed;
            _acceleration = data.Acceleration;
            _decceleration = data.Decceleration;
            _velPower = data.VelPower;

            _playerRb = controller.GetComponent<Rigidbody2D>();
        }

        public override void EnterState()
        {
            base.EnterState();
            currentData = playerData;
            playerController.CurrentSpeed = _moveSpeed;
        }

        public override void DoState()
        {
            base.DoState();
            var isPlayerMove = playerController.MovementDirection.x != 0 
                    || MovementValue >= 1.3f || MovementValue <= -1.3f;

            if (isPlayerMove) return;
            playerStateController.SwitchState(playerController.IdleState);
        }

        public override void DoFixedState()
        {
            base.DoFixedState();
            PlayerMove();
        }
        
        public override void ExitState()
        {
            base.ExitState();
            _playerRb.velocity = Vector2.zero;
        }

        #region Methods

        private void PlayerMove()
        {            
            var direction = playerController.InputHandler.MoveDirection;
            var speed = playerController.CurrentSpeed;

            playerController.MovementDirection = new Vector2(direction.x, playerController.MovementDirection.y);
            playerController.MovementDirection.Normalize();

            var targetSpeed = playerController.MovementDirection * speed;
            var speedDif = targetSpeed.x - _playerRb.velocity.x;
            var accelRate = (Mathf.Abs(targetSpeed.x) > 0.01f) ? _acceleration : _decceleration;
            MovementValue = Mathf.Pow(MathF.Abs(speedDif) * accelRate, _velPower) * MathF.Sign(speedDif);
            
            _playerRb.AddForce(MovementValue * Vector2.right);
        }

        #endregion
    }
}