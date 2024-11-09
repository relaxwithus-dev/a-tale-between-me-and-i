using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI
{
    public class Quest
    {
        // static info
        public QuestInfoSO info;

        // state info
        public QuestStateEnum state;
        private int currentQuestStepIndex;
        private QuestStepState[] questStepStates;

        public Quest(QuestInfoSO questInfo)
        {
            this.info = questInfo;
            this.state = QuestStateEnum.Can_Start;
            this.currentQuestStepIndex = 0;
            this.questStepStates = new QuestStepState[info.questStepPrefabs.Length];
            for (int i = 0; i < questStepStates.Length; i++)
            {
                questStepStates[i] = new QuestStepState();
            }
        }

        public Quest(QuestInfoSO questInfo, QuestStateEnum questState, int currentQuestStepIndex, QuestStepState[] questStepStates)
        {
            this.info = questInfo;
            this.state = questState;
            this.currentQuestStepIndex = currentQuestStepIndex;
            this.questStepStates = questStepStates;

            // if the quest step states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.
            if (this.questStepStates.Length != this.info.questStepPrefabs.Length)
            {
                Debug.LogWarning("Quest Step Prefabs and Quest Step States are "
                    + "of different lengths. This indicates something changed "
                    + "with the QuestInfo and the saved data is now out of sync. "
                    + "Reset your data - as this might cause issues. QuestId: " + this.info.QuestId);
            }
        }

        public void MoveToNextStep()
        {
            currentQuestStepIndex++;
        }

        public bool CurrentStepExists()
        {
            return (currentQuestStepIndex < info.questStepPrefabs.Length);
        }

        public void InstantiateCurrentQuestStep(Transform parentTransform)
        {
            GameObject questStepPrefab = GetCurrentQuestStepPrefab();
            if (questStepPrefab != null)
            {
                QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                    .GetComponent<QuestStep>();
                questStep.InitializeQuestStep(info.QuestId, currentQuestStepIndex, questStepStates[currentQuestStepIndex].state);
            }
        }

        private GameObject GetCurrentQuestStepPrefab()
        {
            GameObject questStepPrefab = null;
            if (CurrentStepExists())
            {
                questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
            }
            else
            {
                Debug.LogWarning("Tried to get quest step prefab, but stepIndex was out of range indicating that "
                    + "there's no current step: QuestId=" + info.QuestId + ", stepIndex=" + currentQuestStepIndex);
            }
            return questStepPrefab;
        }

        public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
        {
            if (stepIndex < questStepStates.Length)
            {
                questStepStates[stepIndex].state = questStepState.state;
                questStepStates[stepIndex].status = questStepState.status;
            }
            else
            {
                Debug.LogWarning("Tried to access quest step data, but stepIndex was out of range: "
                    + "Quest Id = " + info.QuestId + ", Step Index = " + stepIndex);
            }
        }

        // public QuestData GetQuestData()
        // {
        //     return new QuestData(state, currentQuestStepIndex, questStepStates);
        // }

        public string GetFullStatusText()
        {
            string fullStatus = "";

            if (state == QuestStateEnum.Can_Start)
            {
                fullStatus = "This quest can be started!";
            }
            else
            {
                // display all previous quests with strikethroughs
                for (int i = 0; i < currentQuestStepIndex; i++)
                {
                    fullStatus += "<s>" + questStepStates[i].status + "</s>\n";
                }
                // display the current step, if it exists
                if (CurrentStepExists())
                {
                    fullStatus += questStepStates[currentQuestStepIndex].status;
                }
                // when the quest is completed or turned in
                if (state == QuestStateEnum.Can_Finish)
                {
                    fullStatus += "The quest is ready to be turned in.";
                }
                else if (state == QuestStateEnum.Finished)
                {
                    fullStatus += "The quest has been completed!";
                }
            }

            return fullStatus;
        }
    }
}
