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
        protected float MovementValue { get; private set; }
        protected Vector2 playerDirection;
       
        
        // Base Components
        protected float startTime;
        protected string animationName;
        protected bool isExitingState;

         // Reference
        protected PlayerController playerController;
        protected PlayerStateSwitcher playerStateController;

        #endregion

        #region Base Methods
        
        // Constructor
        public PlayerStateBase(PlayerController controller, PlayerStateSwitcher stateController, string animationName)
        {
            this.playerController = controller;
            this.playerStateController = stateController;
            this.animationName = animationName;

            isRight = playerController.PlayerData.IsRight;
        }
        
        public virtual void EnterState() 
        {
            playerController.PlayerAnimator.Play(animationName);
            startTime = Time.time;
            isExitingState = false;
            Debug.Log($"play anim {animationName}");
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

        // !-- Core Functionality
        protected void PlayerMove(float speed, float acceleration, float decceleration)
        {
            var playerRb = playerController.GetComponent<Rigidbody2D>();

            playerDirection = new Vector2(playerController.PlayerInputHandler.Direction.x, playerDirection.y);
            playerDirection.Normalize();

            var targetSpeed = playerDirection * speed;
            var speedDif = targetSpeed.x - playerRb.velocity.x;
            var accelRate = (Mathf.Abs(targetSpeed.x) > 0.01f) ? acceleration : decceleration;
            MovementValue = Mathf.Pow(MathF.Abs(speedDif) * accelRate, playerController.PlayerData.VelPower) * MathF.Sign(speedDif);
            
            playerRb.AddForce(MovementValue * Vector2.right);
        }

        protected void PlayerDirection()
        {
            var direction = playerDirection;
            if (direction.x > 0 && !isRight || direction.x < 0 && isRight) PlayerFlip();
        }

        protected void PlayerFlip()
        {
            isRight = !isRight;
            playerController.transform.Rotate(0f, 180f, 0f);
        }

        #endregion
    }
}