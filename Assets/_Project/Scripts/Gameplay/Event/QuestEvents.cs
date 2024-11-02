using System;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public static class QuestEvents
    {

        public static event Action<int> StartQuest;
        public static event Action<int> AdvanceQuest;
        public static event Action<int> FinishQuest;
        public static event Action<Quest> QuestStateChange;
        public static event Action<int, int, QuestStepState> QuestStepStateChange;

        public static void StartQuestEvent(int id) => StartQuest?.Invoke(id);
        public static void AdvanceQuestEvent(int id) => AdvanceQuest?.Invoke(id);
        public static void FinishQuestEvent(int id) => FinishQuest?.Invoke(id);
        public static void QuestStateChangeEvent(Quest quest) => QuestStateChange?.Invoke(quest);
        public static void QuestStepStateChangeEvent(int id, int stepIndex, QuestStepState questStepState) => QuestStepStateChange?.Invoke(id, stepIndex, questStepState);
    }
}
