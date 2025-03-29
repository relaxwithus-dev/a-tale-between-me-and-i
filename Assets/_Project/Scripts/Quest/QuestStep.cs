using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI
{
    public abstract class QuestStep : MonoBehaviour
    {
        protected int questId;
        protected string targetScene;
        private bool isFinished = false;
        private int stepIndex;

        public void InitializeQuestStep(int questId, int stepIndex, string questStepState, string gameScene)
        {
            this.questId = questId;
            this.stepIndex = stepIndex;
            this.targetScene = gameScene;
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

        protected void ChangeState(string newState, QuestStepStatusEnum newStatus)
        {
            QuestEvents.QuestStepStateChangeEvent(questId, stepIndex, new QuestStepState(newState, newStatus));
        }

        // to set after loading quest step
        protected abstract void SetQuestStepState(string state);
    }
}
