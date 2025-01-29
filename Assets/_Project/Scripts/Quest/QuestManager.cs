using System.Collections;
using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using ATBMI.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ATBMI
{
    public class QuestManager : MonoBehaviour
    {
        // [Header("Config")]
        // [SerializeField] private bool loadQuestState = true;
        [SerializeField] private QuestInfoListSO questInfoList;
        [SerializeField] private GameObject uiQuestPanel;
        [SerializeField] private TextMeshProUGUI uiQuestDisplay;

        private Dictionary<int, Quest> questDataDict = new();

        // quest start requirements
        // private Scene whichScene;??

        public static QuestManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            PopulateQuestDataDict();

            uiQuestPanel.SetActive(false);
        }

        private void OnEnable()
        {
            QuestEvents.StartQuest += StartQuest;
            QuestEvents.AdvanceQuest += AdvanceQuest;
            QuestEvents.FinishQuest += FinishQuest;

            QuestEvents.QuestStepStateChange += QuestStepStateChange;
        }

        private void OnDisable()
        {
            QuestEvents.StartQuest -= StartQuest;
            QuestEvents.AdvanceQuest -= AdvanceQuest;
            QuestEvents.FinishQuest -= FinishQuest;

            QuestEvents.QuestStepStateChange -= QuestStepStateChange;
        }

        private void Start()
        {
            foreach (Quest quest in questDataDict.Values)
            {
                // initialize any loaded quest steps
                if (quest.state == QuestStateEnum.In_Progress)
                {
                    quest.InstantiateCurrentQuestStep(this.transform);
                }
            }
        }

        private void PopulateQuestDataDict()
        {
            foreach (QuestInfoSO questInfo in questInfoList.QuestInfoList)
            {
                if (questDataDict.ContainsKey(questInfo.QuestId))
                {
                    Debug.LogWarning("Duplicate ID found when creating quest data dictionary: " + questInfo.QuestId);
                }
                else
                {
                    questDataDict.Add(questInfo.QuestId, new Quest(questInfo)); // TODO: change the new quest into loaded quests
                }
            }
        }

        private void ChangeQuestState(int id, QuestStateEnum state)
        {
            Quest quest = GetQuestById(id);
            quest.state = state;
        }

        // if any requirements
        // private bool CheckRequirementsMet(Quest quest)
        // {
        //     // start true and prove to be false
        //     bool meetsRequirements = true;

        //     // check player level requirements (e.g player requirement)
        //     if (currentPlayerLevel < quest.info.levelRequirement)
        //     {
        //         meetsRequirements = false;
        //     }

        //     // check quest prerequisites for completion
        //     foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        //     {
        //         if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
        //         {
        //             meetsRequirements = false;
        //         }
        //     }

        //     return meetsRequirements;
        // }

        // Test
        private void Update()
        {
            // Test
            if (Input.GetKeyDown(KeyCode.G))
            {
                StartQuest(1); // go to market quest id
            }

            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     FinishQuest(1);
            // }

            // loop through ALL quests
            // foreach (Quest quest in questMap.Values)
            // {
            //     // if we're now meeting the requirements, switch over to the CAN_START state
            //     if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            //     {
            //         ChangeQuestState(quest.info.id, QuestState.CAN_START);
            //     }
            // }
        }

        private void StartQuest(int id)
        {
            Quest quest = GetQuestById(id);

            if (quest != null && quest.state.Equals(QuestStateEnum.Can_Start))
            {
                quest.InstantiateCurrentQuestStep(this.transform);
                ChangeQuestState(quest.info.QuestId, QuestStateEnum.In_Progress);

                Debug.Log("Quest " + quest.info.displayName + " Started");

                // TODO: change this method to UI manager
                StartCoroutine(AnimateUIQuestPanel(quest, false));
            }
            else
            {
                Debug.Log("Quest " + quest.info.displayName + " Can't be started because its state is " + quest.state + ", it should be " + QuestStateEnum.Can_Start.ToString());
            }
        }

        private void AdvanceQuest(int id)
        {
            Quest quest = GetQuestById(id);

            // move on to the next step
            quest.MoveToNextStep();

            // if there are more steps, instantiate the next one
            if (quest.CurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform);
            }
            // if there are no more steps, then we've finished all of them for this quest
            else
            {
                ChangeQuestState(quest.info.QuestId, QuestStateEnum.Can_Finish);

                if (!quest.info.shoulBeFinishManually)
                {
                    FinishQuest(id);
                }
            }
        }

        private void FinishQuest(int id)
        {
            Quest quest = GetQuestById(id);

            if (quest != null && quest.state.Equals(QuestStateEnum.Can_Finish))
            {
                ClaimRewards(quest);
                ChangeQuestState(quest.info.QuestId, QuestStateEnum.Finished);

                Debug.Log("Quest " + quest.info.displayName + " already finished");

                // TODO: change this method to UI manager
                StartCoroutine(AnimateUIQuestPanel(quest, true));
            }
            else
            {
                Debug.Log("Quest " + quest.info.displayName + " Can't be finished because its state is " + quest.state + ", it should be " + QuestStateEnum.Can_Finish.ToString());
            }
        }

        private void ClaimRewards(Quest quest)
        {
            foreach (var reward in quest.info.rewardItem)
            {
                InventoryManager.Instance.AddItemToInventory(reward.ItemId);
            }
        }

        private IEnumerator AnimateUIQuestPanel(Quest quest, bool isFinished)
        {
            uiQuestPanel.SetActive(true);

            if (isFinished)
            {
                uiQuestDisplay.text = quest.info.displayName + " Selesai";
            }
            else
            {
                uiQuestDisplay.text = quest.info.displayName + " Dimulai";
            }

            yield return new WaitForSeconds(2f);

            uiQuestPanel.SetActive(false);
        }

        private void QuestStepStateChange(int id, int stepIndex, QuestStepState questStepState)
        {
            Quest quest = GetQuestById(id);
            quest.StoreQuestStepState(questStepState, stepIndex);
            ChangeQuestState(id, quest.state);
        }

        public Quest GetQuestById(int id)
        {
            Quest quest = questDataDict[id];
            if (quest == null)
            {
                Debug.LogError("ID not found in the Quest Data Dictionary: " + id);
            }
            return quest;
        }

        private void OnApplicationQuit()
        {
            foreach (Quest quest in questDataDict.Values)
            {
                QuestData questData = quest.GetQuestData();
                Debug.Log(quest.info.QuestId + " " + quest.info.displayName);
                Debug.Log("State = " + questData.state);
                Debug.Log("Index = " + questData.questStepIndex);
                foreach (QuestStepState stepState in questData.questStepStates)
                {
                    Debug.Log("Step State = " + stepState.state);
                }

            }
        }
    }
}
