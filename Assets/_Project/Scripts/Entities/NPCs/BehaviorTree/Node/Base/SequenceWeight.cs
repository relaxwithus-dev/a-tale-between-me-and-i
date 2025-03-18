using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class SequenceWeight : Sequence, IEmotionable
    {
        // Constructor
        public SequenceWeight(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }
        
        // Core
        public float GetRiskValue(Emotion emotion)
        {
            var totalRisk = 1f;
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable risk)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return 0f;
                }
                totalRisk *= 1 - risk.GetRiskValue(emotion);
            }
            return 1 - totalRisk;
        }
        
        public float GetPlanningValue(Emotion emotion)
        {
            var totalPlanning = 0f;
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable planning)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return 0f;
                }
                totalPlanning += planning.GetPlanningValue(emotion);
            }
            return totalPlanning;
        }
        
        public (float, float) GetTimeRange(Emotion emotion)
        {
            var minTime = 0f;
            var maxTime = 0f;
            
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable timeRange)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return (0f, 0f);
                }
                var (L, U) = timeRange.GetTimeRange(emotion);
                minTime += L;
                maxTime += U;
            }
            return (minTime, maxTime);
        }
    }
}