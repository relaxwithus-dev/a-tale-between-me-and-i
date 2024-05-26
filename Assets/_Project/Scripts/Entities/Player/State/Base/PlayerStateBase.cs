using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.Player
{
    public class PlayerStateBase
    {
        #region Base Fields and Properties

        // Components
        protected bool isRight = true;
        protected Vector2 playerDirection;
        protected float MovementValue { get; set; }
       
        
        // Base Components
        protected float startTime;
        protected string animationName;
        protected bool isExitingState;

         // Reference
        protected PlayerController playerController;
        protected PlayerStateSwitcher playerStateController;
        protected SpriteRenderer playerSprite;

        #endregion

        #region Base Methods
        
        // Constructor
        public PlayerStateBase(PlayerController controller, PlayerStateSwitcher stateController, string animationName)
        {
            this.playerController = controller;
            this.playerStateController = stateController;
            this.animationName = animationName;

            isRight = playerController.PlayerData.IsRight;
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
            var direction = playerDirection;
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