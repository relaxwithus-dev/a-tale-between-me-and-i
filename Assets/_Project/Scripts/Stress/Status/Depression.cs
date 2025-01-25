using UnityEngine;
using ATBMI.Data;
using ATBMI.Entities.Player;

namespace ATBMI.Stress
{
    public class Depression : StressStatus
    {
        private float _finalSpeed;

        public Depression(StressData data, PlayerController player, Animator animator) : base(data, player, animator)
        {
            
        }

         // TODO: Drop logic productivity lainnya disini
        public override void PerformStatus()
        {
            base.PerformStatus();

            // Debuff speed
            var speedDebuff = CalculatePercentage(playerController.CurrentSpeed, speedPercentage);
            _finalSpeed = playerController.CurrentSpeed - speedDebuff;
            playerController.CurrentSpeed = _finalSpeed;   
        }
        
        public override void ResetStatus()
        {
            base.ResetStatus();
            _finalSpeed = 0f;
            playerController.CurrentSpeed = playerController.CurrentData.MoveSpeed;  
        }
    }
}