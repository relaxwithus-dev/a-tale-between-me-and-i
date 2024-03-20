using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class PlayerStateBase : MonoBehaviour
    {
        #region Base Fields and Properties

        // Main
        protected PlayerController playerController;
        protected PlayerStateController playerStateController;
        
        // Components
        protected float startTime;
        protected string animationName;
        protected bool isExitingState;

        #endregion

        #region Base Methods

        public void InitializeState(PlayerController controller, PlayerStateController stateController, string animationName)
        {
            this.playerController = controller;
            this.playerStateController = stateController;
            this.animationName = animationName;
        }
        
        public virtual void EnterState() 
        {
            playerController.PlayerAnim.Play(animationName);
            startTime = Time.time;
            isExitingState = false;
            Debug.Log($"play anim {animationName}");
        }

        public virtual void DoState() { }

        public virtual void DoFixedState() { }

        public virtual void ExitState() 
        { 
            isExitingState = true;
        }

        #endregion
    }
}