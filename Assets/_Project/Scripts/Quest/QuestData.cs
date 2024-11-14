/// for saving the quest data

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI
{
    [System.Serializable]
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
