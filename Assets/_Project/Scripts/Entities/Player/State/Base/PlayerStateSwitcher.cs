using System;
using System.Collections.Generic;
using ATBMI.Enum;

namespace ATBMI.Entities.Player
{
    public class PlayerStateSwitcher
    {
        public PlayerStateBase CurrentState { get; private set; }

        public void Initialize(PlayerStateBase startingState)
        {
            CurrentState = startingState;
            CurrentState.EnterState();
        }

        public void SwitchState(PlayerStateBase newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}