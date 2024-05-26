using System;
using System.Collections.Generic;
using ATBMI.Data;
using ATBMI.Entities.Player;
using UnityEngine;

namespace ATBMI.Stress
{
    public class StressStatus
    {
        #region Global Fields
        
        protected float speedRate;
        protected AudioClip stressAuido;
        protected TextAsset dialougeAsset;
        protected Animator stressAnimator;
        protected PlayerController playerController;

        #endregion

        #region Methods

        public StressStatus(StressData data, PlayerController player)
        {
            speedRate = data.StressSpeedRate;
            stressAuido = data.StressAudio;
            dialougeAsset = data.DialogueAsset;
            playerController = player;
        }

        public StressStatus(StressData data, PlayerController player, Animator animator)
        {
            speedRate = data.StressSpeedRate;
            stressAuido = data.StressAudio;
            dialougeAsset = data.DialogueAsset;
            
            playerController = player;
            stressAnimator = animator;
        }

        public virtual void PerformStatus() { }
        
        #endregion
    }
}