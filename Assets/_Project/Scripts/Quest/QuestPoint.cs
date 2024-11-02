using System.Collections;
using System.Collections.Generic;
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

        private bool playerIsNear = false;
        private int questId;
        private QuestStateEnum currentQuestState;

        private void Awake()
        {
            questId = questInfoForPoint.QuestId;
        }

        private void OnEnable()
        {
            QuestEvents.QuestStateChange += QuestStateChange;
        }

        private void OnDisable()
        {
            QuestEvents.QuestStateChange -= QuestStateChange;
        }

        private void Update()
        {
            // TODO: change with interaction manager
            if (GameInputHandler.Instance.IsTapInteract)
            {
                Interact();
            }
        }

        private void Interact()
        {
            if (!playerIsNear)
            {
                return;
            }

            // start or finish a quest
            if (currentQuestState.Equals(QuestStateEnum.Can_Start) && startPoint)
            {
                QuestEvents.StartQuestEvent(questId);
            }
            else if (currentQuestState.Equals(QuestStateEnum.Can_Finish) && finishPoint)
            {
                QuestEvents.FinishQuestEvent(questId);
            }
        }

        private void QuestStateChange(Quest quest)
        {
            // only update the quest state if this point has the corresponding quest
            if (quest.info.QuestId.Equals(questId))
            {
                currentQuestState = quest.state;
            }
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.CompareTag("Player"))
            {
                playerIsNear = true;
            }
        }

        private void OnTriggerExit2D(Collider2D otherCollider)
        {
            if (otherCollider.CompareTag("Player"))
            {
                playerIsNear = false;
            }
        }
    }
}
