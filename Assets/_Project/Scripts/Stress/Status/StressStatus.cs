using UnityEngine;
using ATBMI.Data;
using ATBMI.Player;

namespace ATBMI.Stress
{
    public class StressStatus
    {
        #region Global Fields
        
        protected float speedPercentage;
        protected AudioClip stressAuido;
        protected TextAsset dialougeAsset;
        protected Animator stressAnimator;
        protected PlayerController playerController;

        #endregion

        #region Methods

        // Constructor
        public StressStatus(StressData data, PlayerController player)
        {
            speedPercentage = data.SpeedPercentage;
            stressAuido = data.StressAudio;
            dialougeAsset = data.DialogueAsset;
            playerController = player;
        }

        public StressStatus(StressData data, PlayerController player, Animator animator)
        {
            speedPercentage = data.SpeedPercentage;
            stressAuido = data.StressAudio;
            dialougeAsset = data.DialogueAsset;
            
            playerController = player;
            stressAnimator = animator;
        }

        // Methods
        public virtual void PerformStatus() { }
        public virtual void AvoidStatus() { }

        protected float CalculatePercentage(float value, float percentage)
        {
            return value * (percentage / 100f);
        }
        
        #endregion
    }
}