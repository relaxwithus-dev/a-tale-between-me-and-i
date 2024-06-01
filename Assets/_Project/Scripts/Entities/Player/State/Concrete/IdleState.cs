using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class IdleState : PlayerStateBase
    {
        public IdleState(PlayerController controller, PlayerStateSwitcher stateController, string animationName) : base(controller, stateController, animationName)
        {

        }
        
        public override void DoState()
        {
            base.DoState();
            
            if (playerController.InputHandler.MoveDirection == Vector2.zero) return;
            playerStateController.SwitchState(playerController.WalkState);
        }
    }
}