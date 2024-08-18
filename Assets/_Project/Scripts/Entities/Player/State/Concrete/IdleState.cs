using System;
using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class IdleState : PlayerStateBase
    {
        public IdleState(PlayerControllers controller, PlayerStateSwitcher state, string animationName) : base(controller, state, animationName)
        {

        }

        public override void EnterState()
        {
            base.EnterState();
            currentData = playerController.PlayerData[0];
        }

        public override void DoState()
        {
            base.DoState();
            
            if (playerController.InputHandler.MoveDirection == Vector2.zero) return;
            playerStateController.SwitchState(playerController.WalkState);
        }
    }
}