using ATBMI.Gameplay.Event;
using UnityEngine.InputSystem;
using UnityEngine;
using ATBMI.Gameplay.Handler;

namespace ATBMI
{
    public class QuestPoint : MonoBehaviour
    {
        [Header("Quest")]
        [SerializeField] private QuestInfoSO questInfoForPoint;

        [Header("Config")]
        [SerializeField] private bool startPoint = true;
        [SerializeField] private bool finishPoint = true;

        public bool playerIsNear = false;
        private int questId;
        private QuestStateEnum currentQuestState;

        private void Awake()
        {
            questId = questInfoForPoint.QuestId;
        }

        private void OnEnable()
        {
            QuestEvents.QuestStateChange += QuestStateChange;
            QuestEvents.QuestInteract += QuestInteract;
        }

        private void OnDisable()
        {
            QuestEvents.QuestStateChange -= QuestStateChange;
            QuestEvents.QuestInteract -= QuestInteract;
        }

        // private void Update()
        // {
        //     // TODO: change with interaction manager
        //     if (GameInputHandler.Instance.IsTapInteract)
        //     {
        //         Interact();
        //     }
        // }

        private void QuestInteract(QuestStateEnum questState)
        {
            if (!playerIsNear)
            {
                return;
            }

            // start or finish a quest
            if (questState.Equals(QuestStateEnum.Can_Start) && currentQuestState.Equals(QuestStateEnum.Can_Start) && startPoint)
            {
                QuestEvents.StartQuestEvent(questId);
            }
            else if (questState.Equals(QuestStateEnum.Can_Finish) && currentQuestState.Equals(QuestStateEnum.Can_Finish) && finishPoint)
            {
                QuestEvents.FinishQuestEvent(questId);
            }
        }

        private void QuestStateChange(Quest quest)
        {
            // only update the quest state if this point has the corresponding quest
            if (quest.info.QuestId.Equals(questId))
            {
                Debug.Log("Current Quest State of " + quest.info.displayName + " is " + quest.state);

                currentQuestState = quest.state;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsNear = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsNear = false;
            }
        }
    }
}
