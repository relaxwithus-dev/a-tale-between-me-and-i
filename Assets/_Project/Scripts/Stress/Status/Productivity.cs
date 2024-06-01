using System;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Entities.Player;

namespace ATBMI.Stress
{
    public class Productivity : StressStatus
    {
        public Productivity(StressData data, PlayerController player) : base(data, player)
        {
            
        }

        // TODO: Drop logic buat productivity status disini
        public override void PerformStatus()
        {
            base.PerformStatus();
            Debug.Log("perform productivity");
            var buffPercentage = CalculatePercentage(playerController.CurrentSpeed, speedPercentage);
            playerController.CurrentSpeed += buffPercentage;
        }
    }
}