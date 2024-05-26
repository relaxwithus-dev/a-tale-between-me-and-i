using System;
using System.Collections.Generic;
using ATBMI.Data;
using ATBMI.Entities.Player;
using UnityEngine;

namespace ATBMI.Stress
{
    public class Depression : StressStatus
    {
        public Depression(StressData data, PlayerController player, Animator animator) : base(data, player, animator)
        {
            
        }

        public override void PerformStatus()
        {
            base.PerformStatus();
        }

    }
}