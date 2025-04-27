using UnityEngine;
using System;
using ATBMI.Scene;

namespace ATBMI
{
    public class AreaArrivalQuestStep : QuestStep
    {
        private bool hasArrived;

        private void ArrivedAtTargetArea()
        {
            if (!hasArrived)
            {
                hasArrived = true;
                UpdateStepState(QuestStepStatusEnum.Finished);
                FinishQuestStep();
            }
        }

        private void UpdateStepState(QuestStepStatusEnum stepStatus)
        {
            ChangeState(hasArrived.ToString(), stepStatus);
        }

        protected override void SetQuestStepState(string state)
        {
            hasArrived = Convert.ToBoolean(state);
            UpdateStepState(hasArrived ? QuestStepStatusEnum.Finished : QuestStepStatusEnum.In_Progress);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (SceneNavigation.Instance.CurrentScene.name == targetScene && other.CompareTag("Player"))
            {
                ArrivedAtTargetArea();
            }
        }
    }
}
