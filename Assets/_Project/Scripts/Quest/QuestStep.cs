using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI.Quest
{
    public abstract class QuestStep : MonoBehaviour
    {
        protected int questId;
        protected string targetScene;
        private bool _isFinished;
        private int _stepIndex;

        public void InitializeQuestStep(int questId, int stepIndex, string questStepState, string gameScene)
        {
            this.questId = questId;
            _stepIndex = stepIndex;
            targetScene = gameScene;
            if (questStepState != null && questStepState != "")
            {
                SetQuestStepState(questStepState);
            }
        }

        protected void FinishQuestStep()
        {
            if (!_isFinished)
            {
                _isFinished = true;

                QuestEvents.AdvanceQuestEvent(questId);

                Destroy(this.gameObject);
            }
        }

        protected void ChangeState(string newState, QuestStepStatusEnum newStatus)
        {
            QuestEvents.QuestStepStateChangeEvent(questId, _stepIndex, new QuestStepState(newState, newStatus));
        }

        // to set after loading quest step
        protected abstract void SetQuestStepState(string state);
    }
}
