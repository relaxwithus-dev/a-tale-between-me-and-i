using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class WalkState : PlayerStateBase
    {
        #region Internal Fields

        private float _walkSpeed;
        private float _acceleration;
        private float _decceleration;

        #endregion

        public WalkState(PlayerController controller, PlayerStateSwitcher stateController, string animationName) : base(controller, stateController, animationName)
        {

        }

        public override void EnterState()
        {
            base.EnterState();

            var walkStats = playerController.PlayerData.WalkStats;
            _walkSpeed = walkStats.MoveSpeed;
            _acceleration = walkStats.Acceleration;
            _decceleration = walkStats.Decceleration;
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
            PlayerMove(_walkSpeed, _acceleration, _decceleration);
        }

        public override void ExitState()
        {
            base.ExitState();
            playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}