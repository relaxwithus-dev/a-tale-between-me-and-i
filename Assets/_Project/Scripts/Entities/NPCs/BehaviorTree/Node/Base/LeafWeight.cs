using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class LeafWeight : Leaf, IEmotionable
    {
        private Dictionary<Emotion, (float plan, float risk, (float, float) timeRange)> _emotionFactors = new();
        
        // Constructor
        protected LeafWeight() : base() { }
        public LeafWeight(string nodeName) : base(nodeName) { }
        
        // Core
        protected void OverrideEmotionFactors(Dictionary<Emotion, (float, float, (float, float))> factors)
        {
            _emotionFactors = factors;
        }
        
        public float GetRiskValue(Emotion emotion)
        {
            if (_emotionFactors.TryGetValue(emotion, out var factor))
                return factor.risk;
            
            return 0f;
        }
        
        public float GetPlanningValue(Emotion emotion)
        {
            if (_emotionFactors.TryGetValue(emotion, out var factor))
                return factor.plan;
            
            return 0f;
        }
        
        public (float, float) GetTimeRange(Emotion emotion)
        {
            if (_emotionFactors.TryGetValue(emotion, out var factor))
                return factor.timeRange;
            
            return (0f, 0f);
        }
    }
}