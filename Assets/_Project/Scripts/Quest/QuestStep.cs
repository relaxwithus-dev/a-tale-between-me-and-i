using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI
{
    public abstract class QuestStep : MonoBehaviour
    {
        private bool isFinished = false;
        private int questId;
        private int stepIndex;

        public void InitializeQuestStep(int questId, int stepIndex, string questStepState)
        {
            this.questId = questId;
            this.stepIndex = stepIndex;
            if (questStepState != null && questStepState != "")
            {
                SetQuestStepState(questStepState);
            }
        }

        protected void FinishQuestStep()
        {
            if (!isFinished)
            {
                isFinished = true;

                QuestEvents.AdvanceQuestEvent(questId);

                Destroy(this.gameObject);
            }
        }

        protected void ChangeState(string newState, string newStatus)
        {
            QuestEvents.QuestStepStateChangeEvent(questId, stepIndex, new QuestStepState(newState, newStatus));
        }

        protected abstract void SetQuestStepState(string state);
    }
}
