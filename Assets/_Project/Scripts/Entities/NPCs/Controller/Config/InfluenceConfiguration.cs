using System;
using ATBMI.Enum;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [CreateAssetMenu(fileName = "NewInfluenceConfig", menuName = "Data/Entities/Config/Influence Configuration", order = 1)]
    public class InfluenceConfiguration : ScriptableObject
    {
        [Serializable]
        public struct InfluenceData
        {
            public InteractTypes Types;
            public float[] Values;
        }
        
        [SerializeField] private InfluenceData[] actionInfluences;
        
        public float[] GetInfluenceValues(InteractTypes type)
        {
            foreach (var influence in actionInfluences)
            {
                if (influence.Types == type)
                    return influence.Values;
            }
            return null;
        }
    }
}