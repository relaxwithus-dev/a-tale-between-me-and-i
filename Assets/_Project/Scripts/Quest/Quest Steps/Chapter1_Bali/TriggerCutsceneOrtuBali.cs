using UnityEngine;
using System;

namespace ATBMI
{
    public class TriggerCutsceneOrtuBali : QuestStep
    {
        private bool hasArrivedAtNearOrtu;

        private void ArrivedAtNearOrtu()
        {
            if (!hasArrivedAtNearOrtu)
            {
                hasArrivedAtNearOrtu = true;

                UpdateStepState(QuestStepStatusEnum.Finished);

                Quest quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(hasArrivedAtNearOrtu.ToString(), stepStatus); // TODO: change to new status
        }

        protected override void SetQuestStepState(string state)
        {
            this.hasArrivedAtNearOrtu = Convert.ToBoolean(state);

            if (hasArrivedAtNearOrtu)
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
                ArrivedAtNearOrtu();
            }
        }
    }
}
