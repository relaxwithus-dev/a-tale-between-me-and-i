using System.Collections;
using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using UnityEngine;

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

                Quest quest = QuestManager.Instance.GetQuestById(questId);
                FinishQuestStep();

                Debug.Log(quest.GetFullStatusText());
            }
        }

        protected override void SetQuestStepState(string state)
        {

        }
    }
}
