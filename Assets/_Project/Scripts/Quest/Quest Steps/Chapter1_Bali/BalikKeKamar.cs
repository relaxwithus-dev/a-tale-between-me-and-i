using UnityEngine;
using System;

namespace ATBMI
{
    public class BalikKeKamar : QuestStep
    {
        private bool hasArrivedAtKamarBali;

        private void ArrivedAtKamarBali()
        {
            if (!hasArrivedAtKamarBali)
            {
                hasArrivedAtKamarBali = true;

                UpdateStepState(QuestStepStatusEnum.Finished);

                Quest quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(hasArrivedAtKamarBali.ToString(), stepStatus); // TODO: change to new status
        }

        protected override void SetQuestStepState(string state)
        {
            this.hasArrivedAtKamarBali = Convert.ToBoolean(state);

            if (hasArrivedAtKamarBali)
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
                ArrivedAtKamarBali();
            }
        }
    }
}
