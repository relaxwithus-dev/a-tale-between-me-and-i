using ATBMI.Gameplay.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ATBMI
{
    public class QuestStepLog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stepText;
        [SerializeField] private Image statusBackground;
        [SerializeField] private Image statusIcon; //  To visually indicate step completion

        [Space(10)]
        [SerializeField] private Sprite incompleteStatusBackground;
        [SerializeField] private Sprite CompletedStatusBackground;
        [SerializeField] private Sprite incompleteStatusIcon;
        [SerializeField] private Sprite CompletedStatusIcon;

        public void SetText(string text)
        {
            stepText.text = text;
        }

        public void UpdateStepStatus(QuestStepStatusEnum status)
        {
            switch (status)
            {
                case QuestStepStatusEnum.In_Progress:
                    statusBackground.sprite = incompleteStatusBackground;
                    statusIcon.sprite = incompleteStatusIcon;
                    stepText.fontStyle = FontStyles.Normal;
                    break;
                case QuestStepStatusEnum.Finished:
                    statusBackground.sprite = CompletedStatusBackground;
                    statusIcon.sprite = CompletedStatusIcon;
                    stepText.fontStyle = FontStyles.Strikethrough;
                    break;
                default:
                    break;
            }
        }
    }
}
