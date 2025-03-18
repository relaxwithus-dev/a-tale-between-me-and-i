namespace ATBMI.Entities.NPCs
{
    public interface IEmotionable
    {
        // Factors Method
        public float GetRiskValue(Emotion emotion);
        public float GetPlanningValue(Emotion emotion);
        public (float, float) GetTimeRange(Emotion emotion);
    }
}