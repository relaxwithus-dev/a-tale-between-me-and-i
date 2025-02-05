using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class WeightLeaf : WeightNode
    {
        // Factors
        private readonly float riskFactor;
        private readonly float planningFactor;
        private readonly (float, float) timeFactor;
        
        public WeightLeaf(float riskFactor, float planningFactor, (float, float) timeFactor)
        {
            this.riskFactor = riskFactor;
            this.planningFactor = planningFactor;
            this.timeFactor = timeFactor;
        }
        
        public override float GetRiskValue() => riskFactor;
        public override float GetPlanningValue() => planningFactor;
        public override (float, float) GetTimeRange() => timeFactor;
    }
}