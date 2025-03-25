using ATBMI.Enum;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [CreateAssetMenu(fileName = "NewInfluenceConfig", menuName = "Data/Entities/Config/Influence Configuration", order = 1)]
    public class InfluenceConfiguration : ScriptableObject
    {
        private float[,] actionInfluence = {
            { 0.3f, 0.2f, 0, 0 },    // is_talking
            { -0.2f, -0.1f, -0.3f, 0.1f }, // is_running
            { 0.3f, 0.2f, 0.1f, 0 },  // is_walk
            { 0.3f, 0.3f, -0.2f, 0.1f }, // is_give_item
            { -0.2f, -0.1f, 0.1f, 0 }  // is_take_item
        };
        
        public float[] GetInfluenceValues(InteractAction action)
        {
            var tempValues = new float[4];
            var actionCount = System.Enum.GetValues(typeof(InteractAction)).Length;
            
            for (var i = 0; i < actionCount; i++)
            {
                if (i != (int)action) continue;
                for (var j = 0; j < 4; j++)
                {
                    tempValues[j] = actionInfluence[i, j];
                }
                break;
            }
            
            return tempValues;
        }
    }
}