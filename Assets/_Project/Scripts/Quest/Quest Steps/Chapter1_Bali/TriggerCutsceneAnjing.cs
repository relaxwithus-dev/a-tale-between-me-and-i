using UnityEngine;
using System;

namespace ATBMI
{
    public class TriggerCutsceneAnjing : QuestStep
    {
        private bool hasArrivedAtFrontGate;

        private void ArrivedAtFrontGate()
        {
            if (!hasArrivedAtFrontGate)
            {
                hasArrivedAtFrontGate = true;

                UpdateStepState(QuestStepStatusEnum.Finished);

                Quest quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(hasArrivedAtFrontGate.ToString(), stepStatus); // TODO: change to new status
        }

        protected override void SetQuestStepState(string state)
        {
            this.hasArrivedAtFrontGate = Convert.ToBoolean(state);

            if (hasArrivedAtFrontGate)
            {
                UpdateStepState(QuestStepStatusEnum.Finished);
            }
            else
            {
                UpdateStepState(QuestStepStatusEnum.In_Progress);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // arrived
                ArrivedAtFrontGate();
            }
        }
    }
}
