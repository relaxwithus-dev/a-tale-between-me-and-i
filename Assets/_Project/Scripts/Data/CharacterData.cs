using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Data/Entities/Character Data", order = 1)]
    public class CharacterData : ScriptableObject
    {
        [Serializable]
        private struct CharacterMoves
        {
            public string Type;
            public float Speed;
        }
        
        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] private bool isIdling;
        [SerializeField] [HideIf("isIdling")] 
        private CharacterMoves[] moveSpeeds;
        
        // Getter
        public string CharacterName => characterName;
        public float GetSpeedByType(string type)
        {
            foreach (var speed in moveSpeeds)
            {
                if (speed.Type == type)
                    return speed.Speed;
            }
                
            Debug.LogError("move speeds not found!");
            return 0f;
        }
    }
}
