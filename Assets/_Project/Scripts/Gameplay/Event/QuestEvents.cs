using System;
using ATBMI.Entities.NPCs;
using ATBMI.Interaction;
using ATBMI.Quest;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public static class QuestEvents
    {
        // Main Event
        public static event Action<int> StartQuest;
        public static event Action<int> AdvanceQuest;
        public static event Action<int> FinishQuest;
        public static event Action<QuestBase> QuestStateChange;
        public static event Action<int, int, QuestStepState> QuestStepStateChange;

        // Quest Step Event
        // TODO: put quest step event here
        public static event Action<int> GetItemQuestStep;
        public static event Action<UIMenuTabEnum> CheckUIMenuTabQuestStep;

        // Additional Quest Event
        public static event Action InitiateRegisterNPCs;
        public static event Action<CharacterAI> RegisterThisNPCToHandledByQuestStep;
        public static event Action UnregisterNPCsThatHandledByQuestStep;
        public static event Action<ItemInteract> RegisterThisItemToHandledByQuestStep;
        public static event Action UnregisterItemsThatHandledByQuestStep;

        // Main Caller
        public static void StartQuestEvent(int id) => StartQuest?.Invoke(id);
        public static void AdvanceQuestEvent(int id) => AdvanceQuest?.Invoke(id);
        public static void FinishQuestEvent(int id) => FinishQuest?.Invoke(id);
        public static void QuestStateChangeEvent(QuestBase quest) => QuestStateChange?.Invoke(quest);
        public static void QuestStepStateChangeEvent(int id, int stepIndex, QuestStepState questStepState) => QuestStepStateChange?.Invoke(id, stepIndex, questStepState);

        internal static void QuestStateChangeEvent(QuestBase quest, object status)
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
        public static void RegisterThisItemToHandledByQuestStepEvent(ItemInteract itemInteract) => RegisterThisItemToHandledByQuestStep?.Invoke(itemInteract);
        public static void UnregisterItemsThatHandledByQuestStepEvent() => UnregisterItemsThatHandledByQuestStep?.Invoke();
    }
}
