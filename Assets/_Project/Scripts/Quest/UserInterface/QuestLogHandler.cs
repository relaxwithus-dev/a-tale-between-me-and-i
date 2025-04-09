using System;
using System.Collections;
using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ATBMI
{
    public class QuestLogHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private GameObject activeContentParent;
        [SerializeField] private GameObject completedContentParent;
        [SerializeField] private Transform questStepParent;
        // [SerializeField] private QuestLogScrollingList scrollingList;
        // [SerializeField] private TextMeshProUGUI questDisplayNameText;

        [Header("Rect Transforms")]
        [SerializeField] private RectTransform scrollRectTransform;
        [SerializeField] private RectTransform contentRectTransform;

        [Header("Quest Log Button")]
        [SerializeField] private GameObject questLogButtonPrefab;
        [SerializeField] private GameObject questStepLogPrefab;

        [Header("Input Actions")]
        [SerializeField] private InputActionReference navigateAction; // This is a Vector2 input

        // private Button firstSelectedButton;
        private int selectedIndex;
        private bool isQuestLogOpen = false;
        private List<QuestLogButton> questLogButtons = new();
        private List<QuestStepLog> questStepLogs = new(); // List to track all instantiated quest step logs

        private void Awake()
        {
            selectedIndex = -1;
        }

        private void OnEnable()
        {
            QuestEvents.QuestStateChange += QuestStateChange;
            navigateAction.action.performed += OnNavigate;

            UIEvents.OnSelectTabQuest += OpenQuestLog;
            UIEvents.OnDeselectTabQuest += CloseQuestLog;
        }

        private void OnDisable()
        {
            QuestEvents.QuestStateChange -= QuestStateChange;
            navigateAction.action.performed -= OnNavigate;

            UIEvents.OnSelectTabQuest -= OpenQuestLog;
            UIEvents.OnDeselectTabQuest -= CloseQuestLog;
        }

        private void OpenQuestLog()
        {
            if (questLogButtons.Count > 0)
            {
                selectedIndex = 0;
                HighlightButton(selectedIndex);

                // Ensure quest steps are shown for the first quest
                Quest selectedQuest = FindQuestById(questLogButtons[selectedIndex].QuestId);
                if (selectedQuest != null)
                {
                    UpdateQuestSteps(selectedQuest);
                }
            }
            isQuestLogOpen = true;
        }


        private void CloseQuestLog()
        {
            ResetSelection();
            isQuestLogOpen = false;
        }

        private void ResetSelection()
        {
            if (selectedIndex >= 0 && questLogButtons.Count > 0)
            {
                HighlightButton(selectedIndex, false);
            }
            selectedIndex = -1;
        }

        private void HighlightButton(int index, bool highlight = true)
        {
            if (questLogButtons.Count == 0) return;

            QuestLogButton button = GetQuestLogButtonAt(index);
            if (button != null)
            {
                button.Highlight(highlight);
            }
        }

        private QuestLogButton GetQuestLogButtonAt(int index)
        {
            if (index < 0 || index >= questLogButtons.Count) return null;
            return questLogButtons[index];
        }

        private void QuestStateChange(Quest quest)
        {
            // add the button to the scrolling list if not already added
            QuestLogButton questLogButton = CreateButtonIfNotExists(quest);

            // set the button color based on quest state
            questLogButton.SetState(quest.state);

            // Move the button to the appropriate content parent based on quest state
            UpdateTransformParent(questLogButton, quest);

            // Update all quest step logs related to this quest
            UpdateQuestSteps(quest);
        }

        private QuestLogButton CreateButtonIfNotExists(Quest quest)
        {
            // Check if the quest already has a button
            QuestLogButton existingButton = questLogButtons.Find(button => button.QuestId == quest.info.QuestId);

            if (existingButton != null)
            {
                return existingButton;
            }

            // Instantiate a new button if it doesn't exist
            return InstantiateQuestLogButton(quest);
        }

        private QuestLogButton InstantiateQuestLogButton(Quest quest)
        {
            // Instantiate the quest log button
            QuestLogButton questLogButton = Instantiate(questLogButtonPrefab, activeContentParent.transform)
                .GetComponent<QuestLogButton>();

            questLogButton.Initialize(quest.info.displayName, quest.info.QuestId);

            // Add to list
            questLogButtons.Add(questLogButton);

            return questLogButton;
        }

        private void UpdateTransformParent(QuestLogButton questLogButton, Quest quest)
        {
            Transform targetParent = (quest.state == QuestStateEnum.Finished) ? completedContentParent.transform : activeContentParent.transform;

            // Move the button only if its parent needs to change
            if (questLogButton.transform.parent != targetParent)
            {
                int originalIndex = questLogButtons.IndexOf(questLogButton); // Get the original order based on quest start order

                questLogButton.transform.SetParent(targetParent, false);

                // Maintain order: insert at the correct index instead of pushing to the top
                questLogButton.transform.SetSiblingIndex(originalIndex + 1); // +1 for completed text on the scene
            }
        }

        public void UpdateQuestSteps(Quest quest)
        {
            int maxSteps = quest.info.questSteps.Length;
            int currentQuestStepIndex = Mathf.Clamp(quest.CurrentQuestStepIndex, 0, maxSteps); // Prevent out-of-bounds

            // Ensure at least the first quest step log is instantiated
            if (questStepLogs.Count == 0 && maxSteps > 0)
            {
                InstantiateQuestStepLog(quest, 0);
            }

            // Activate or instantiate only up to the current quest step index
            for (int i = 0; i <= currentQuestStepIndex && i < maxSteps; i++)
            {
                QuestStepLog stepLog;

                if (i < questStepLogs.Count)
                {
                    // Reuse existing log
                    stepLog = questStepLogs[i];
                    stepLog.gameObject.SetActive(true);
                }
                else
                {
                    // Instantiate new log
                    InstantiateQuestStepLog(quest, i);
                }

                // Update state of quest step log
                string stepStatus = quest.info.questSteps[i].stepStatusText;
                stepLog = questStepLogs[i];
                stepLog.SetText(stepStatus);
                stepLog.UpdateStepStatus(quest.GetQuestData().questStepStates[i].status);
            }

            // Deactivate excess logs if any
            for (int i = currentQuestStepIndex + 1; i < questStepLogs.Count; i++)
            {
                questStepLogs[i].gameObject.SetActive(false);
            }
        }

        private void InstantiateQuestStepLog(Quest quest, int stepIndex)
        {
            GameObject stepObject = Instantiate(questStepLogPrefab, questStepParent);
            QuestStepLog stepLog = stepObject.GetComponent<QuestStepLog>();
            questStepLogs.Add(stepLog);
        }


        private void OnNavigate(InputAction.CallbackContext context)
        {
            if (!isQuestLogOpen) return;

            Vector2 input = context.ReadValue<Vector2>();

            if (input.y > 0.5f) // Up
            {
                Navigate(-1);
            }
            else if (input.y < -0.5f) // Down
            {
                Navigate(1);
            }
        }

        private void Navigate(int direction)
        {
            if (questLogButtons.Count == 0) return;

            HighlightButton(selectedIndex, false);
            selectedIndex = (selectedIndex + direction + questLogButtons.Count) % questLogButtons.Count;
            HighlightButton(selectedIndex);
            UpdateScrolling(GetQuestLogButtonAt(selectedIndex)?.GetComponent<RectTransform>());

            // Ensure quest steps are updated based on the new selection
            Quest selectedQuest = FindQuestById(questLogButtons[selectedIndex].QuestId);
            if (selectedQuest != null)
            {
                UpdateQuestSteps(selectedQuest);
            }
        }

        private Quest FindQuestById(int questId)
        {
            return QuestManager.Instance.GetQuestById(questId);
        }


        private void UpdateScrolling(RectTransform buttonRectTransform)
        {
            // calculate the min and max for the selected button
            float buttonYMin = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
            float buttonYMax = buttonYMin + buttonRectTransform.rect.height;

            // calculate the min and max for the content area
            float contentYMin = contentRectTransform.anchoredPosition.y;
            float contentYMax = contentYMin + scrollRectTransform.rect.height;

            // handle scrolling down
            if (buttonYMax > contentYMax)
            {
                contentRectTransform.anchoredPosition = new Vector2(
                    contentRectTransform.anchoredPosition.x,
                    buttonYMax - scrollRectTransform.rect.height
                );
            }
            // handle scrolling up
            else if (buttonYMin < contentYMin)
            {
                contentRectTransform.anchoredPosition = new Vector2(
                    contentRectTransform.anchoredPosition.x,
                    buttonYMin
                );
            }
        }
    }
}
