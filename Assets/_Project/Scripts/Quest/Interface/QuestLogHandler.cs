using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ATBMI.Gameplay.Event;
using UnityEngine.UI;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Quest
{
    public class QuestLogHandler : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private VerticalLayoutGroup contentButton;
        // [SerializeField] private GameObject activeContentParent;
        // [SerializeField] private GameObject completedContentParent;
        [SerializeField] private Transform questStepParent;
        // [SerializeField] private QuestLogScrollingList scrollingList;
        // [SerializeField] private TextMeshProUGUI questDisplayNameText;

        [Header("Rect Transforms")]
        [SerializeField] private RectTransform scrollRectTransform;
        [SerializeField] private RectTransform contentRectTransform;

        [Header("Quest Log Button")]
        [SerializeField] private GameObject questLogButtonPrefab;
        [SerializeField] private GameObject questStepLogPrefab;

        // [Header("Input Actions")]
        // [SerializeField] private InputActionReference navigateAction; // This is a Vector2 input

        [Header("References")]
        [SerializeField] private GameObject questTab;

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
            // navigateAction.action.performed += OnNavigate;

            UIEvents.OnSelectTabQuest += OpenQuestLog;
            UIEvents.OnDeselectTabQuest += CloseQuestLog;
        }

        private void OnDisable()
        {
            QuestEvents.QuestStateChange -= QuestStateChange;
            // navigateAction.action.performed -= OnNavigate;

            UIEvents.OnSelectTabQuest -= OpenQuestLog;
            UIEvents.OnDeselectTabQuest -= CloseQuestLog;
        }

        private void Update()
        {
            if (!questTab.activeInHierarchy) return;

            if (GameInputHandler.Instance.IsArrowDown)
            {
                Navigate(1);
            }
            else if (GameInputHandler.Instance.IsArrowUp)
            {
                Navigate(-1);
            }
        }

        private void OpenQuestLog()
        {
            if (questLogButtons.Count > 0)
            {
                // contentButton.enabled = false;

                selectedIndex = 0;
                HighlightButton(selectedIndex);

                // Ensure quest steps are shown for the first quest
                QuestBase selectedQuest = FindQuestById(questLogButtons[selectedIndex].QuestId);
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

        private void QuestStateChange(QuestBase quest)
        {
            // add the button to the scrolling list if not already added
            QuestLogButton questLogButton = CreateButtonIfNotExists(quest);

            // set the button color based on quest state
            questLogButton.SetState(quest.state);

            // Move the button to the appropriate content parent based on quest state
            // UpdateTransformParent(questLogButton, quest);

            // Reorder the quest log button index
            // ReorderQuestLogButtonsByHierarchy();

            // Update all quest step logs related to this quest
            UpdateQuestSteps(quest);
        }

        private QuestLogButton CreateButtonIfNotExists(QuestBase quest)
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

        private QuestLogButton InstantiateQuestLogButton(QuestBase quest)
        {
            // Instantiate the quest log button
            QuestLogButton questLogButton = Instantiate(questLogButtonPrefab, contentButton.transform)
                .GetComponent<QuestLogButton>();

            questLogButton.Initialize(quest.info.displayName, quest.info.QuestId);

            // Add to list
            questLogButtons.Add(questLogButton);

            return questLogButton;
        }

        // private void UpdateTransformParent(QuestLogButton questLogButton, QuestBase quest)
        // {
        //     Transform targetParent = (quest.state == QuestStateEnum.Finished) ? completedContentParent.transform : activeContentParent.transform;

        //     // Move the button only if its parent needs to change
        //     if (questLogButton.transform.parent != targetParent)
        //     {
        //         int originalIndex = questLogButtons.IndexOf(questLogButton); // Get the original order based on quest start order

        //         questLogButton.transform.SetParent(targetParent, false);

        //         // Maintain order: insert at the correct index instead of pushing to the top
        //         questLogButton.transform.SetSiblingIndex(originalIndex + 1); // +1 for completed text on the scene
        //     }
        // }

        public void UpdateQuestSteps(QuestBase quest)
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

        private void InstantiateQuestStepLog(QuestBase quest, int stepIndex)
        {
            GameObject stepObject = Instantiate(questStepLogPrefab, questStepParent);
            QuestStepLog stepLog = stepObject.GetComponent<QuestStepLog>();
            questStepLogs.Add(stepLog);
        }


        // private void OnNavigate(InputAction.CallbackContext context)
        // {
        //     if (!isQuestLogOpen) return;

        //     Vector2 input = context.ReadValue<Vector2>();

        //     if (input.y > 0.5f) // Up
        //     {
        //         Navigate(-1);
        //     }
        //     else if (input.y < -0.5f) // Down
        //     {
        //         Navigate(1);
        //     }
        // }

        private void Navigate(int direction)
        {
            if (questLogButtons.Count == 0) return;

            HighlightButton(selectedIndex, false);

            selectedIndex = (selectedIndex + direction + questLogButtons.Count) % questLogButtons.Count;
            HighlightButton(selectedIndex);

            // Ensure quest steps are updated based on the new selection
            QuestBase selectedQuest = FindQuestById(questLogButtons[selectedIndex].QuestId);
            if (selectedQuest != null)
            {
                UpdateQuestSteps(selectedQuest);
            }

            // UpdateScrolling(GetQuestLogButtonAt(selectedIndex)?.GetComponent<RectTransform>());
            UpdateScrolling(questLogButtons[selectedIndex].transform as RectTransform);
        }

        private void UpdateScrolling(RectTransform buttonRectTransform)
        {
            // calculate the min and max for the selected button
            float buttonTop = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
            float buttonBottom = buttonTop + buttonRectTransform.rect.height;

            // calculate the min and max for the content area
            float contentTop = contentRectTransform.anchoredPosition.y;
            float contentBottom = contentTop + scrollRectTransform.rect.height;

            // handle scrolling down
            if (buttonBottom > contentBottom)
            {
                contentRectTransform.anchoredPosition = new Vector2(
                    contentRectTransform.anchoredPosition.x,
                    buttonBottom - scrollRectTransform.rect.height
                );
            }
            // handle scrolling up
            else if (buttonTop < contentTop)
            {
                contentRectTransform.anchoredPosition = new Vector2(
                    contentRectTransform.anchoredPosition.x,
                    buttonTop - 34.34833f // change the value according to the height of the button
                );
            }
        }

        private QuestBase FindQuestById(int questId)
        {
            return QuestManager.Instance.GetQuestById(questId);
        }

        // public void ReorderQuestLogButtonsByHierarchy()
        // {
        //     questLogButtons.Clear();

        //     // First, collect active quests
        //     foreach (Transform child in activeContentParent.transform)
        //     {
        //         QuestLogButton button = child.GetComponent<QuestLogButton>();
        //         if (button != null)
        //         {
        //             questLogButtons.Add(button);
        //         }
        //     }

        //     // Then, collect completed quests
        //     foreach (Transform child in completedContentParent.transform)
        //     {
        //         QuestLogButton button = child.GetComponent<QuestLogButton>();
        //         if (button != null)
        //         {
        //             questLogButtons.Add(button);
        //         }
        //     }
        // }


        public void ClearQuestLog()
        {
            // Destroy all QuestLogButton GameObjects and clear the list
            foreach (var button in questLogButtons)
            {
                if (button != null)
                    Destroy(button.gameObject);
            }
            questLogButtons.Clear();

            // Destroy all QuestStepLog GameObjects and clear the list
            foreach (var stepLog in questStepLogs)
            {
                if (stepLog != null)
                    Destroy(stepLog.gameObject);
            }
            questStepLogs.Clear();

            // Reset selection index and UI state
            selectedIndex = -1;
            isQuestLogOpen = false;
        }

    }
}
