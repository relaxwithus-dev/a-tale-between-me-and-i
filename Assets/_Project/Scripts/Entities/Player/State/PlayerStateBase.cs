using System;
using System.Collections.Generic;

namespace ATBMI.Entities.Player
{
    public abstract class PlayerStateBase
    {
        public abstract void EnterState(PlayerController playerController);
        public abstract void FixedUpdateState(PlayerController playerController);
        public abstract void UpdateState(PlayerController playerController);
    }
}