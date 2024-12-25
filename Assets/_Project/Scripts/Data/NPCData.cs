using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewNPCData", menuName = "Data/NPC Data", order = 0)]
    public class NPCData : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private float moveSpeed;
        
        // Getter
        public string PlayerName => characterName;
        public float MoveSpeed => moveSpeed;
    }
}
