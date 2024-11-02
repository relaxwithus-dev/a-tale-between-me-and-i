using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI
{
    public class GoToMarketQuestStep : QuestStep
    {
        private bool hasArrivedAtMarket;

        private void ArrivedAtMarket()
        {
            if(!hasArrivedAtMarket)
            {
                hasArrivedAtMarket = true;

                FinishQuestStep();
            }
        }

        protected override void SetQuestStepState(string state)
        {
            
        }
    }
}
