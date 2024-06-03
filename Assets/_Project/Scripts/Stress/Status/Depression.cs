using System;
using System.Collections.Generic;
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

        // TODO: Drop logic buat depression status disini
        public override void PerformStatus()
        {
            base.PerformStatus();
            Debug.Log("perform depression");

            if (playerController.CurrentSpeed != _finalSpeed)
            {
                var debuffPercentage = CalculatePercentage(playerController.CurrentSpeed, speedPercentage);
                _finalSpeed = playerController.CurrentSpeed - debuffPercentage;
                playerController.CurrentSpeed = _finalSpeed;   
            }
        }

        public override void AvoidStatus()
        {
            base.AvoidStatus();
            Debug.Log("avoid depression");
            var currentData = playerController.StateSwitcher.CurrentState.CurrentData;

            _finalSpeed = 0f;
            playerController.CurrentSpeed = currentData.MoveSpeed;   
        }

    }
}