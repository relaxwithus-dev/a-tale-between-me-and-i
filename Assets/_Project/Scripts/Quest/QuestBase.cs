using UnityEngine;

namespace ATBMI.Quest
{
    public class QuestBase
    {
        // static info
        public QuestInfoSO info;
        public int CurrentQuestStepIndex => currentQuestStepIndex;
        
        // state info
        public QuestStateEnum state;
        private int currentQuestStepIndex;
        private QuestStepState[] questStepStates;

        public QuestBase(QuestInfoSO info)
        {
            this.info = info;
            state = QuestStateEnum.Can_Start;
            currentQuestStepIndex = 0;
            questStepStates = new QuestStepState[this.info.questSteps.Length];
            
            for (var i = 0; i < questStepStates.Length; i++)
            {
                questStepStates[i] = new QuestStepState();
            }
        }

        public QuestBase(QuestInfoSO info, QuestStateEnum state, int currentQuestStepIndex, QuestStepState[] questStepStates)
        {
            this.info = info;
            this.state = state;
            this.currentQuestStepIndex = currentQuestStepIndex;
            this.questStepStates = questStepStates;

            // if the quest step states and prefabs are different lengths,
            // something has changed during development and the saved data is out of sync.
            if (this.questStepStates.Length != this.info.questSteps.Length)
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

        public void ResetQuestStep()
        {
            currentQuestStepIndex = 0;
        }
        
        public bool CurrentStepExists()
        {
            return (currentQuestStepIndex < info.questSteps.Length);
        }

        public bool IsNextStepExists()
        {
            return (currentQuestStepIndex +1 < info.questSteps.Length);
        }

        public void InstantiateCurrentQuestStep(Transform parentTransform)
        {
            GameObject questStepPrefab = GetCurrentQuestStepPrefab();
            if (questStepPrefab != null)
            {
                QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform)
                    .GetComponent<QuestStep>();
                questStep.InitializeQuestStep(info.QuestId, currentQuestStepIndex,
                    questStepStates[currentQuestStepIndex].state, info.questSteps[currentQuestStepIndex].targetScene);
            }
        }

        public GameObject GetCurrentQuestStepPrefab()
        {
            GameObject questStepPrefab = null;
            if (CurrentStepExists())
            {
                questStepPrefab = info.questSteps[currentQuestStepIndex].questStepPrefab;
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

        public QuestData GetQuestData()
        {
            return new QuestData(state, currentQuestStepIndex, questStepStates);
        }

        // TODO: use it as quest log
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
