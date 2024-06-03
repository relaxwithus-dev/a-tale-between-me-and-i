using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class JumpState : PlayerStateBase
    {
        public JumpState(PlayerController controller, PlayerStateSwitcher stateController, string animationName) : base(controller, stateController, animationName)
        {
            // TODO: Isi data jika perlu, misal ga perlu kosongi aja
        }
        
        public override void EnterState()
        {
            base.EnterState();
            playerController.CurrentSpeed = latestSpeed;
        }
        
        public override void DoState()
        {
            base.DoState();
        }
        
        public override void ExitState()
        {
            base.ExitState();
        }
    }
}