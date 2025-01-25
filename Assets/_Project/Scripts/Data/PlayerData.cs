using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Data/Entities/Player Data", order = 0)]
    public class PlayerData : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private string playerName;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float deceleration;
        
        // Getter
        public string PlayerName => playerName;
        public float MoveSpeed => moveSpeed;
        public float Deceleration => deceleration;

    }
}
