using System;

namespace ATBMI.Quest
{
    [Serializable]
    public class QuestData
    {
        public QuestStateEnum state;
        public int questStepIndex;
        public QuestStepState[] questStepStates;

        public QuestData(QuestStateEnum state, int questStepIndex, QuestStepState[] questStepStates)
        {
            this.state = state;
            this.questStepIndex = questStepIndex;
            this.questStepStates = questStepStates;
        }
    }
}
