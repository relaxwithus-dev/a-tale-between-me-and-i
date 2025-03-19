using UnityEngine;
using System;

namespace ATBMI
{
    public class PergiKeTebing : QuestStep
    {
        private bool hasArrivedAtTebing;

        private void ArrivedAtTebing()
        {
            if (!hasArrivedAtTebing)
            {
                hasArrivedAtTebing = true;

                UpdateStepState(QuestStepStatusEnum.Finished);

                Quest quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(hasArrivedAtTebing.ToString(), stepStatus); // TODO: change to new status
        }

        protected override void SetQuestStepState(string state)
        {
            this.hasArrivedAtTebing = Convert.ToBoolean(state);

            if (hasArrivedAtTebing)
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
                ArrivedAtTebing();
            }
        }
    }
}
