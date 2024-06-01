using System;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Entities.Player;

namespace ATBMI.Stress
{
    public class Depression : StressStatus
    {
        public Depression(StressData data, PlayerController player, Animator animator) : base(data, player, animator)
        {
            
        }

        // TODO: Drop logic buat depression status disini
        public override void PerformStatus()
        {
            base.PerformStatus();
            Debug.Log("perform depression");
            var debuffPercentage = CalculatePercentage(playerController.CurrentSpeed, speedPercentage);
            playerController.CurrentSpeed += debuffPercentage;
        }

    }
}