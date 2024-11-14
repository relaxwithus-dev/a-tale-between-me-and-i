using System;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public static class QuestEvents
    {
        // Main Event
        public static event Action<int> StartQuest;
        public static event Action<int> AdvanceQuest;
        public static event Action<int> FinishQuest;
        public static event Action<int, int, QuestStepState> QuestStepStateChange;

        // Quest Step Event
        public static event Action ArrivedAtMarket;

        // Main Caller
        public static void StartQuestEvent(int id) => StartQuest?.Invoke(id);
        public static void AdvanceQuestEvent(int id) => AdvanceQuest?.Invoke(id);
        public static void FinishQuestEvent(int id) => FinishQuest?.Invoke(id);
        public static void QuestStepStateChangeEvent(int id, int stepIndex, QuestStepState questStepState) => QuestStepStateChange?.Invoke(id, stepIndex, questStepState);

        // Quest Step Caller
        public static void ArrivedAtMarketEvent() => ArrivedAtMarket?.Invoke();
    }
}
