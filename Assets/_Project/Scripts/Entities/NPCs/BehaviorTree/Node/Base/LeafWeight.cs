namespace ATBMI.Entities.NPCs
{
    public class LeafWeight : Leaf, IEmotionable
    {
        // Factors
        private float risk;
        private float planning;
        private (float, float) timeRange;
        
        // Methods
        public LeafWeight() : base() { }
        public LeafWeight(string nodeName) : base(nodeName) { }

        protected void InitFactors(float planning, float risk, (float, float) timeRange)
        {
            this.risk = risk;
            this.planning = planning;
            this.timeRange = timeRange;
        }
        
        public float GetRiskValue() => risk;
        public float GetPlanningValue() => planning;
        public (float, float) GetTimeRange() => timeRange;
    }
}