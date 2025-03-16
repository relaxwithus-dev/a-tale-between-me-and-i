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
        public static event Action<Quest> QuestStateChange;
        public static event Action<int, int, QuestStepState> QuestStepStateChange;

        // Quest Step Event
        // TODO: put quest step event here

        // Main Caller
        public static void StartQuestEvent(int id) => StartQuest?.Invoke(id);
        public static void AdvanceQuestEvent(int id) => AdvanceQuest?.Invoke(id);
        public static void FinishQuestEvent(int id) => FinishQuest?.Invoke(id);
        public static void QuestStateChangeEvent(Quest quest) => QuestStateChange?.Invoke(quest);
        public static void QuestStepStateChangeEvent(int id, int stepIndex, QuestStepState questStepState) => QuestStepStateChange?.Invoke(id, stepIndex, questStepState);

        internal static void QuestStateChangeEvent(Quest quest, object status)
        {
            throw new NotImplementedException();
        }

        // Quest Step Caller

    }
}
