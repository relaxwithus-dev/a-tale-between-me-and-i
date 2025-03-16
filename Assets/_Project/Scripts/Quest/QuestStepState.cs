namespace ATBMI
{
    [System.Serializable]
    public class QuestStepState
    {
        public string state;
        public QuestStepStatusEnum status;

        public QuestStepState(string state, QuestStepStatusEnum status)
        {
            this.state = state;
            this.status = status;
        }

        public QuestStepState()
        {
            this.state = "";
            this.status = QuestStepStatusEnum.Null;
        }
    }
}
