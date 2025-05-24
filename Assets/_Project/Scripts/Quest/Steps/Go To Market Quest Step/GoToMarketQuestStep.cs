using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ATBMI.Quest
{
    public class GoToMarketQuestStep : QuestStep
    {
        private bool hasArrivedAtMarket;

        private void ArrivedAtMarket()
        {
            if (!hasArrivedAtMarket)
            {
                hasArrivedAtMarket = true;

                UpdateStepState(QuestStepStatusEnum.Finished);

                QuestBase quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(hasArrivedAtMarket.ToString(), stepStatus); // TODO: change to new status
        }
        
        protected override void SetQuestStepState(string state)
        {
            hasArrivedAtMarket = Convert.ToBoolean(state);
        
            if (hasArrivedAtMarket)
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
            if (SceneManager.GetActiveScene().name == targetScene && other.CompareTag("Player"))
            {
                // arrived
                ArrivedAtMarket();
            }
        }
    }
}
