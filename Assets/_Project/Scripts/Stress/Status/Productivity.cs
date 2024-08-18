using System;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Data;
using ATBMI.Entities.Player;

namespace ATBMI.Stress
{
    public class Productivity : StressStatus
    {
        private float _finalSpeed;

        public Productivity(StressData data, PlayerControllers player) : base(data, player)
        {
            
        }

        // TODO: Drop logic buat productivity status disini
        public override void PerformStatus()
        {
            base.PerformStatus();
            Debug.Log("perform productivity");

            if (playerController.CurrentSpeed != _finalSpeed)
            {
                var debuffPercentage = CalculatePercentage(playerController.CurrentSpeed, speedPercentage);
                _finalSpeed = playerController.CurrentSpeed + debuffPercentage;
                playerController.CurrentSpeed = _finalSpeed;   
            }
        }

        public override void AvoidStatus()
        {
            base.AvoidStatus();
            Debug.Log("avoid productivity");
            var currentData = playerController.StateSwitcher.CurrentState.CurrentData;

            _finalSpeed = 0f;
            playerController.CurrentSpeed = currentData.MoveSpeed;
        }
    }
}