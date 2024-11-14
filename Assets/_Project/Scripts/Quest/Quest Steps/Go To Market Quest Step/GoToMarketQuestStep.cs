using System.Collections;
using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using UnityEngine;
using System;

namespace ATBMI
{
    public class GoToMarketQuestStep : QuestStep
    {
        private bool hasArrivedAtMarket;

        private void OnEnable()
        {
            QuestEvents.ArrivedAtMarket += ArrivedAtMarket;
        }

        private void OnDisable()
        {
            QuestEvents.ArrivedAtMarket -= ArrivedAtMarket;
        }

        private void ArrivedAtMarket()
        {
            if (!hasArrivedAtMarket)
            {
                hasArrivedAtMarket = true;

                UpdateStepState();

                Quest quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();
            }
        }

        private void UpdateStepState()
        {
            ChangeState(hasArrivedAtMarket.ToString(), hasArrivedAtMarket.ToString()); // TODO: change to new status
        }

        protected override void SetQuestStepState(string state)
        {
            this.hasArrivedAtMarket = Convert.ToBoolean(state);

            UpdateStepState();
        }
    }
}
