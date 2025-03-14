namespace ATBMI.Entities.NPCs
{
    public class LeafWeight : Leaf, IEmotionable
    {
        // Factors
        private float risk;
        private float plan;
        private (float, float) timeRange;
        
        // Constructor
        protected LeafWeight() : base() { }
        public LeafWeight(string nodeName) : base(nodeName) { }
        
        // Core
        protected void InitFactors(float plan, float risk, (float, float) timeRange)
        {
            this.risk = risk;
            this.plan = plan;
            this.timeRange = timeRange;
        }
        
        public float GetRiskValue() => risk;
        public float GetPlanningValue() => plan;
        public (float, float) GetTimeRange() => timeRange;
    }
}