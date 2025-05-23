using System;
using ATBMI.Entities.NPCs;
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
        public static event Action<int> GetItemQuestStep;
        public static event Action<UIMenuTabEnum> CheckUIMenuTabQuestStep;

        // Additional Quest Event
        public static event Action InitiateRegisterNPCs;
        public static event Action<CharacterAI> RegisterThisNPCToHandledByQuestStep;
        public static event Action UnregisterNPCsThatHandledByQuestStep;

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
        public static void GetItemQuestStepEvent(int itemId) => GetItemQuestStep?.Invoke(itemId);
        public static void CheckUIMenuTabQuestStepEvent(UIMenuTabEnum tab) => CheckUIMenuTabQuestStep?.Invoke(tab);

        // Additional Quest Evenet Caller
        public static void InitiateRegisterNPCsEvent() => InitiateRegisterNPCs?.Invoke();
        public static void RegisterThisNPCToHandledByQuestStepEvent(CharacterAI characterAI) => RegisterThisNPCToHandledByQuestStep?.Invoke(characterAI);
        public static void UnregisterNPCsThatHandledByQuestStepEvent() => UnregisterNPCsThatHandledByQuestStep?.Invoke();
    }
}
