using System;
using System.Collections.Generic;
using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class PlayerStateBase
    {
        #region Base Fields and Properties

        // Components
        protected bool isRight = true;
        protected float latestSpeed;
        protected PlayerData currentData;
        protected float MovementValue { get; set; }
        public PlayerData CurrentData => currentData;
        
        // Base Components
        protected float startTime;
        protected string animationName;
        protected bool isExitingState;

         // Reference
        protected PlayerController playerController;
        protected PlayerData playerData;
        protected PlayerStateSwitcher playerStateController;
        protected SpriteRenderer playerSprite;

        #endregion

        #region Base Methods

        // Constructor
        public PlayerStateBase(PlayerController controller, PlayerData data, PlayerStateSwitcher state, string animationName)
        {
            this.playerData = data;
            this.playerController = controller;
            this.playerStateController = state;
            this.animationName = animationName;
            
            isRight = controller.IsRight;
            playerSprite = controller.GetComponentInChildren<SpriteRenderer>();
        }

        public PlayerStateBase(PlayerController controller, PlayerStateSwitcher state, string animationName)
        {
            this.playerController = controller;
            this.playerStateController = state;
            this.animationName = animationName;
            
            isRight = controller.IsRight;
            playerSprite = controller.GetComponentInChildren<SpriteRenderer>();
        }

        public virtual void EnterState() 
        {
            playerController.PlayerAnimator.Play(animationName);
            startTime = Time.time;
            isExitingState = false;
        }

        public virtual void DoState() 
        {
            PlayerDirection();
        }

        public virtual void DoFixedState() { }

        public virtual void ExitState() 
        { 
            isExitingState = true;
        }

        #endregion

        #region Methods

        protected void PlayerDirection()
        {
            var direction = playerController.MovementDirection;
            if (direction.x > 0 && !isRight || direction.x < 0 && isRight) PlayerFlip();
        }
        
        public virtual void PlayerFlip()
        {
            isRight = !isRight;
            playerSprite.flipX = !isRight;
        }

        #endregion
    }
}