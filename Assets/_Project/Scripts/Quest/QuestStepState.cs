using System;

namespace ATBMI.Quest
{
    [Serializable]
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
            state = "";
            status = QuestStepStatusEnum.Null;
        }
    }
}
