using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Entities/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float decceleration;
        [SerializeField] private float velPower;

        public float MoveSpeed => moveSpeed;
        public float Acceleration => acceleration;
        public float Decceleration => decceleration;
        public float VelPower => velPower;

    }
}
