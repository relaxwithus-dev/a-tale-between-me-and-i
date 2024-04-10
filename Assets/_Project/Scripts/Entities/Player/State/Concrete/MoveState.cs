using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class MoveState : PlayerStateBase
    {
        #region Internal Fields
        
        private float _runSpeed;
        private float _acceleration;
        private float _decceleration;

        #endregion

        public MoveState(PlayerController controller, PlayerStateSwitcher stateController, string animationName) : base(controller, stateController, animationName)
        {
            // TODO: Isi data jika perlu, misal ga perlu kosongi aja
        }

        public override void EnterState()
        {
            base.EnterState();
            
            var runStats = playerController.PlayerData.RunStats;
            _runSpeed = runStats.MoveSpeed;
            _acceleration = runStats.Acceleration;
            _decceleration = runStats.Decceleration;
        }

        public override void DoState()
        {
            base.DoState();
            
            var isPlayerMove = playerDirection.x != 0 || MovementValue >= 1.3f || MovementValue <= -1.3f;
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
            playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        #region Methods

        private void PlayerMove()
        {
            var playerRb = playerController.GetComponent<Rigidbody2D>();

            playerDirection = new Vector2(playerController.PlayerInputHandler.Direction.x, playerDirection.y);
            playerDirection.Normalize();

            var targetSpeed = playerDirection * _runSpeed;
            var speedDif = targetSpeed.x - playerRb.velocity.x;
            var accelRate = (Mathf.Abs(targetSpeed.x) > 0.01f) ? _acceleration : _decceleration;
            MovementValue = Mathf.Pow(MathF.Abs(speedDif) * accelRate, playerController.PlayerData.VelPower) * MathF.Sign(speedDif);
            
            playerRb.AddForce(MovementValue * Vector2.right);
        }

        #endregion
    }
}