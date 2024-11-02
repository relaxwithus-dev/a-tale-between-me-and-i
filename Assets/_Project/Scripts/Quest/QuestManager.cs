using System.Collections;
using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using ATBMI.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ATBMI
{
    public class QuestManager : MonoBehaviour
    {
        // [Header("Config")]
        // [SerializeField] private bool loadQuestState = true;
        [SerializeField] private QuestInfoListSO questInfoList;

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
                // broadcast the initial state of all quests on startup
                QuestEvents.QuestStateChangeEvent(quest);
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
            QuestEvents.QuestStateChangeEvent(quest);
        }

        // private bool CheckRequirementsMet(Quest quest)
        // {
        //     // start true and prove to be false
        //     bool meetsRequirements = true;

        //     // check player level requirements
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

        private void Update()
        {
            // // loop through ALL quests
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
            quest.InstantiateCurrentQuestStep(this.transform);
            ChangeQuestState(quest.info.QuestId, QuestStateEnum.In_Progress);
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
            }
        }

        private void FinishQuest(int id)
        {
            Quest quest = GetQuestById(id);
            ClaimRewards(quest);
            ChangeQuestState(quest.info.QuestId, QuestStateEnum.Finished);
        }

        private void ClaimRewards(Quest quest)
        {
            foreach (var reward in quest.info.rewardItem)
            {
                InventoryManager.Instance.AddItemToInventory(reward.ItemId);
            }
        }

        private void QuestStepStateChange(int id, int stepIndex, QuestStepState questStepState)
        {
            Quest quest = GetQuestById(id);
            quest.StoreQuestStepState(questStepState, stepIndex);
            ChangeQuestState(id, quest.state);
        }

        private Quest GetQuestById(int id)
        {
            Quest quest = questDataDict[id];
            if (quest == null)
            {
                Debug.LogError("ID not found in the Quest Data Dictionary: " + id);
            }
            return quest;
        }
    }
}
