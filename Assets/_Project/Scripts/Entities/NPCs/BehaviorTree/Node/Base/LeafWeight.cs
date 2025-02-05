namespace ATBMI.Entities.NPCs
{
    public class LeafWeight : Leaf, IEmotionable
    {
        // Factors
        private readonly float risk;
        private readonly float planning;
        private readonly (float, float) timeRange;
        
        // Methods
        public LeafWeight() : base() { }
        public LeafWeight(string nodeName) : base(nodeName) { }
        public LeafWeight(float risk, float planning, (float, float) timeRange)
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