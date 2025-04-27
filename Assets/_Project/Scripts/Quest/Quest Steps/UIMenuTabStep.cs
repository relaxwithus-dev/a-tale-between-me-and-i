using System;
using ATBMI.Gameplay.Event;
using UnityEngine;

namespace ATBMI
{
    public class UIMenuTabStep : QuestStep
    {
        [SerializeField] private UIMenuTabEnum uIMenuTab = UIMenuTabEnum.Map;
        private bool isCorrectlyOpenedUIMenuTab;

        private void OnEnable()
        {
            QuestEvents.CheckUIMenuTabQuestStep += CheckUIMenuTab;
        }

        private void OnDisable()
        {
            QuestEvents.CheckUIMenuTabQuestStep -= CheckUIMenuTab;
        }

        private void CheckUIMenuTab(UIMenuTabEnum tab)
        {
            if(!isCorrectlyOpenedUIMenuTab && uIMenuTab == tab)
            {
                isCorrectlyOpenedUIMenuTab = true;
                UpdateStepState(QuestStepStatusEnum.Finished);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(isCorrectlyOpenedUIMenuTab.ToString(), stepStatus);
        }

        protected override void SetQuestStepState(string state)
        {
            isCorrectlyOpenedUIMenuTab = Convert.ToBoolean(state);

            UpdateStepState(isCorrectlyOpenedUIMenuTab ? QuestStepStatusEnum.Finished : QuestStepStatusEnum.In_Progress);
        }
    }
}
