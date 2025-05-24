using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using ATBMI.Inventory;
using ATBMI.Gameplay.Event;

namespace ATBMI.Quest
{
    public class QuestManager : MonoBehaviour
    {
        // [Header("Config")]
        // [SerializeField] private bool loadQuestState = true;
        [SerializeField] private QuestInfoListSO questInfoList;
        [SerializeField] private QuestLogHandler questLogHandler;
        [SerializeField] private GameObject uiQuestPanel;
        [SerializeField] private CanvasGroup uiQuestCanvasGroup;
        [SerializeField] private TextMeshProUGUI uiQuestTitle;
        [SerializeField] private TextMeshProUGUI uiQuestStepInfo;

        private Dictionary<int, QuestBase> questDataDict = new();

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

                return;
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
            foreach (QuestBase quest in questDataDict.Values)
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
                    questDataDict.Add(questInfo.QuestId, new QuestBase(questInfo)); // TODO: change the new quest into loaded quests
                }
            }
        }

        private void ChangeQuestState(int id, QuestStateEnum state)
        {
            QuestBase quest = GetQuestById(id);
            quest.state = state;

            QuestEvents.QuestStateChangeEvent(quest);
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
            // if (Input.GetKeyDown(KeyCode.G))
            // {
            //     StartQuest(999); // go to market quest id
            // }
            //
            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     StartQuest(997); // go to market quest id
            // }

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
            QuestBase quest = GetQuestById(id);

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
            QuestBase quest = GetQuestById(id);

            // move on to the next step
            quest.MoveToNextStep();

            // if there are more steps, instantiate the next one
            if (quest.CurrentStepExists())
            {
                quest.InstantiateCurrentQuestStep(this.transform);

                StartCoroutine(AnimateUIQuestPanel(quest, false));
            }
            // if there are no more steps, then we've finished all of them for this quest
            else
            {
                ChangeQuestState(quest.info.QuestId, QuestStateEnum.Can_Finish);

                if (!quest.info.shouldBeFinishManually)
                {
                    FinishQuest(id);
                }
            }
        }

        private void FinishQuest(int id)
        {
            QuestBase quest = GetQuestById(id);

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

        private void ClaimRewards(QuestBase quest)
        {
            foreach (var reward in quest.info.rewardItem)
            {
                InventoryManager.Instance.AddItemToInventory(reward.ItemId);
            }
        }

        private IEnumerator AnimateUIQuestPanel(QuestBase quest, bool isFinished)
        {
            uiQuestPanel.SetActive(true);

            uiQuestCanvasGroup.alpha = 0f;

            if (isFinished)
            {
                uiQuestTitle.text = quest.info.displayName + " Selesai";
                uiQuestStepInfo.text = "";
            }
            else
            {
                uiQuestTitle.text = quest.info.displayName;
                uiQuestStepInfo.text = quest.info.questSteps[quest.CurrentQuestStepIndex].stepStatusText;
            }

            // Fade in
            uiQuestCanvasGroup.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(4f);

            // Fade out
            uiQuestCanvasGroup.DOFade(0f, 0.5f);
            yield return new WaitForSeconds(0.5f);

            uiQuestPanel.SetActive(false);
        }

        private void QuestStepStateChange(int id, int stepIndex, QuestStepState questStepState)
        {
            QuestBase quest = GetQuestById(id);
            quest.StoreQuestStepState(questStepState, stepIndex);
            ChangeQuestState(id, quest.state);
        }

        public QuestBase GetQuestById(int id)
        {
            QuestBase quest = questDataDict[id];
            if (quest == null)
            {
                Debug.LogError("ID not found in the Quest Data Dictionary: " + id);
            }
            return quest;
        }

        // private void OnApplicationQuit()
        // {
        //     foreach (Quest quest in questDataDict.Values)
        //     {
        //         QuestData questData = quest.GetQuestData();
        //         Debug.Log(quest.info.QuestId + " " + quest.info.displayName);
        //         Debug.Log("State = " + questData.state);
        //         Debug.Log("Index = " + questData.questStepIndex);
        //         foreach (QuestStepState stepState in questData.questStepStates)
        //         {
        //             Debug.Log("Step State = " + stepState.state);
        //         }
        //
        //     }
        // }

        public void ResetQuest()
        {
            foreach (QuestBase quest in questDataDict.Values)
            {
                QuestData questData = quest.GetQuestData();
                
                // questData.state = QuestStateEnum.Can_Start;
                // questData.questStepIndex = 0;
                
                ChangeQuestState(quest.info.QuestId, QuestStateEnum.Can_Start);

                quest.ResetQuestStep();
            }
            
            DOTween.Kill(uiQuestPanel);
            uiQuestPanel.SetActive(false);
            
            // Destroy all quest step GameObjects
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            questLogHandler.ClearQuestLog();
        }
    }
}
