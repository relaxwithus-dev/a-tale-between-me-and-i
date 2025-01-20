using UnityEngine;
using ATBMI.Data;
using ATBMI.Entities.Player;

namespace ATBMI.Stress
{
    public class StressStatus
    {
        #region Global Fields
        
        protected float speedPercentage;
        protected AudioClip stressAudio;
        protected TextAsset dialougeAsset;
        protected Animator stressAnimator;
        protected PlayerController playerController;
        
        #endregion

        #region Methods

        // Constructor
        public StressStatus(StressData data, PlayerController player)
        {
            speedPercentage = data.SpeedPercentage;
            stressAudio = data.StressAudio;
            dialougeAsset = data.DialogueAsset;
            playerController = player;
        }

        public StressStatus(StressData data, PlayerController player, Animator animator)
        {
            speedPercentage = data.SpeedPercentage;
            dialougeAsset = data.DialogueAsset;
            stressAudio = data.StressAudio;
            
            playerController = player;
            stressAnimator = animator;
        }

        // Methods
        public virtual void PerformStatus() { }
        public virtual void ResetStatus() { }

        protected float CalculatePercentage(float value, float percentage)
        {
            return value * (percentage / 100f);
        }
        
        #endregion
    }
}