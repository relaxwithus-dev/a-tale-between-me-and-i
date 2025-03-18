using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI
{
    public class GoToRayaQuestStep : QuestStep
    {
        private bool hasArrivedAtRaya;

        private void ArrivedAtMarket()
        {
            if (!hasArrivedAtRaya)
            {
                hasArrivedAtRaya = true;

                UpdateStepState(QuestStepStatusEnum.Finished);

                Quest quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(hasArrivedAtRaya.ToString(), stepStatus); // TODO: change to new status
        }

        protected override void SetQuestStepState(string state)
        {
            this.hasArrivedAtRaya = Convert.ToBoolean(state);

            if (hasArrivedAtRaya)
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
                ArrivedAtMarket();
            }
        }
    }
}
