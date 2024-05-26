using System;
using System.Collections.Generic;
using ATBMI.Data;
using ATBMI.Entities.Player;
using UnityEngine;

namespace ATBMI.Stress
{
    public class Productivity : StressStatus
    {
        public Productivity(StressData data, PlayerController player) : base(data, player)
        {
            
        }

        public override void PerformStatus()
        {
            base.PerformStatus();
            
        }
    }
}