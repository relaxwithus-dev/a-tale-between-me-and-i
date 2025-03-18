using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class SelectorWeight : Selector, IEmotionable
    {
        // Constructor
        public SelectorWeight(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }
        
        // Core
        public float GetRiskValue(Emotion emotion)
        {
            var totalRisk = 0f;
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable risk)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return 0f;
                }
                totalRisk += risk.GetRiskValue(emotion);
            }
            return totalRisk / childNodes.Count;
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
            return totalPlanning / childNodes.Count;
        }
        
        public (float, float) GetTimeRange(Emotion emotion)
        {
            var minTime = float.MaxValue;
            var maxTime = float.MinValue;
            
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable timeRange)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return (0f, 0f);
                }
                var (L, U) = timeRange.GetTimeRange(emotion);
                minTime = Mathf.Min(minTime, L);
                maxTime = Mathf.Max(maxTime, U);
            }
            return (minTime, maxTime);
        }
    }
}