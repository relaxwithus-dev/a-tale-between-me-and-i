using System;
using UnityEngine;
using ATBMI.Entities.Player;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Entities/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Serializable]
        public struct MoveStat
        {
            public PlayerState State;
            public float Speed;
            public float Deceleration;
        }
        
        [Header("Stats")]
        [SerializeField] private string playerName;
        [SerializeField] private string playerAnimationTag;
        [SerializeField] private MoveStat[] moveStats;
        
        [Header("Assets")]
        [SerializeField] private Sprite playerSprite;
        [SerializeField] private RuntimeAnimatorController playerAnimator;
        
        // Getter
        public string PlayerName => playerName;
        public string PlayerAnimationTag => playerAnimationTag;
        public MoveStat[] MoveStats => moveStats;
        
        public Sprite PlayerSprite => playerSprite;
        public RuntimeAnimatorController PlayerAnimator => playerAnimator;

    }
}
