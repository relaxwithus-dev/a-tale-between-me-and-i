namespace ATBMI.Entities.NPCs
{
    public interface IEmotionable
    {
        // Factors Method
        public float GetRiskValue();
        public float GetPlanningValue();
        public (float, float) GetTimeRange();
    }
}