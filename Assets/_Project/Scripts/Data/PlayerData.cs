using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "ScriptableObject/Entities/New Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        #region Struct
        [Serializable]
        public struct SpeedStats
        {
            public float MoveSpeed;
            public float Acceleration;
            public float Decceleration;
        }
        #endregion

        [Header("General")]
        [SerializeField] private string playerName;
        [SerializeField] private bool isRight;

        public string PlayerName => playerName;
        public bool IsRight => isRight;

        [Header("Movement")]
        [SerializeField] private SpeedStats walkStats;
        [SerializeField] private SpeedStats runStats;
        [SerializeField] private float velPower;

        public SpeedStats WalkStats => walkStats;
        public SpeedStats RunStats => runStats;
        public float VelPower => velPower;

    }
}
