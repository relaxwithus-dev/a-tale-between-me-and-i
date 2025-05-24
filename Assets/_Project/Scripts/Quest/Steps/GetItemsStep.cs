using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Quest
{
    public class GetItemsStep : QuestStep
    {
        [SerializeField] private List<int> itemIds;
        private Dictionary<int, bool> itemStates = new();

        private void Awake()
        {
            foreach (int id in itemIds)
            {
                itemStates[id] = false; // All items start as not collected
            }
        }

        private void OnEnable()
        {
            QuestEvents.GetItemQuestStep += GetItem;
        }

        private void OnDisable()
        {
            QuestEvents.GetItemQuestStep -= GetItem;
        }

        private void GetItem(int id)
        {
            if (itemStates.ContainsKey(id))
            {
                itemStates[id] = true;

                if (IsAllItemsCollected())
                {
                    UpdateStepState(QuestStepStatusEnum.Finished);
                    FinishQuestStep();
                }
                else
                {
                    UpdateStepState(QuestStepStatusEnum.In_Progress);
                }
            }
        }

        private bool IsAllItemsCollected()
        {
            return itemStates.Values.All(status => status == true);
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            string state = SerializeItemStates();
            // Debug.Log(state);
            ChangeState(state, stepStatus);
        }

        // Serialize the dictionary into a string. e.g "1:true,2:false,3:true"
        private string SerializeItemStates()
        {
            return string.Join(",", itemStates.Select(pair => $"{pair.Key}:{pair.Value}"));
        }

        // Deserialize the string back to the dictionary
        protected override void SetQuestStepState(string state)
        {
            itemStates = state
                .Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(pair => pair.Split(':'))
                .ToDictionary(
                    parts => int.Parse(parts[0]),
                    parts => bool.Parse(parts[1])
                );

            UpdateStepState(IsAllItemsCollected() ? QuestStepStatusEnum.Finished : QuestStepStatusEnum.In_Progress);
        }
    }
}
