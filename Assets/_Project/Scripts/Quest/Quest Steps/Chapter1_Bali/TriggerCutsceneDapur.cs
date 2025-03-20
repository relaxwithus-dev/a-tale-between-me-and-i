using UnityEngine;
using System;

namespace ATBMI
{
    public class TriggerCutsceneDapur : QuestStep
    {
        private bool hasArrivedAtDapur;

        private void ArrivedAtDapur()
        {
            if (!hasArrivedAtDapur)
            {
                hasArrivedAtDapur = true;

                UpdateStepState(QuestStepStatusEnum.Finished);

                Quest quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(hasArrivedAtDapur.ToString(), stepStatus); // TODO: change to new status
        }

        protected override void SetQuestStepState(string state)
        {
            this.hasArrivedAtDapur = Convert.ToBoolean(state);

            if (hasArrivedAtDapur)
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
                ArrivedAtDapur();
            }
        }
    }
}
