using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Entities/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private float moveSpeed;
        
        // Getter
        public string CharacterName => characterName;
        public float MoveSpeed => moveSpeed;
    }
}
