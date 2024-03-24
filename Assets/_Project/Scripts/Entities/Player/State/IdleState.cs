using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class IdleState : PlayerStateBase
    {
        public IdleState(PlayerController controller, PlayerStateSwitcher stateController, string animationName) : base(controller, stateController, animationName)
        {
            // TODO: Isi data jika perlu, misal ga perlu kosongi aja
        }
        
        public override void EnterState()
        {
            base.EnterState();
        }
        
        public override void DoState()
        {
            base.DoState();
            if (playerController.PlayerInputHandler.Direction != Vector2.zero)
            {
                playerStateController.SwitchState(playerController.WalkState);
                Debug.Log("switch to walk");
            }
        }
        
        public override void ExitState()
        {
            base.ExitState();
        }
    }
}