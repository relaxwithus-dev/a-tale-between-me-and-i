using UnityEngine;
using ATBMI.Data;
using ATBMI.Entities.Player;

namespace ATBMI.Stress
{
    public class Depression : StressStatus
    {
        // Fields
        private float _finalSpeed;

        // Methods
        public Depression(StressData data, PlayerController player, Animator animator) : base(data, player, animator) { }
        
        public override void PerformStatus()
        {
            base.PerformStatus();

            var speedDebuff = CalculatePercentage(playerController.CurrentSpeed, speedPercentage);
            _finalSpeed = playerController.CurrentSpeed - speedDebuff;
            playerController.CurrentSpeed = _finalSpeed;   
        }
        
        public override void ResetStatus()
        {
            base.ResetStatus();
            _finalSpeed = 0f;
            playerController.CurrentSpeed = playerController.CurrentStat.Speed;  
        }
    }
}