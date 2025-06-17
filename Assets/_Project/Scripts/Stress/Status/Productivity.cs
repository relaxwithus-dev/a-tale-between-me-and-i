using UnityEngine;
using ATBMI.Data;
using ATBMI.Entities.Player;

namespace ATBMI.Stress
{
    public class Productivity : StressStatus
    {
        // Fields
        private float _finalSpeed;

        // Methods
        public Productivity(StressData data, PlayerController player) : base(data, player) { }
        
        public override void PerformStatus()
        {
            base.PerformStatus();
            var speedBuff = CalculatePercentage(playerController.CurrentSpeed, speedPercentage);
            _finalSpeed = playerController.CurrentSpeed + speedBuff;
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