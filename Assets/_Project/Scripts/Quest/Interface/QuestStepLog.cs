using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ATBMI.Quest
{
    public class QuestStepLog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stepText;
        [SerializeField] private Image statusBackground;

        [Space(10)]
        [SerializeField] private Sprite incompleteStatusBackground;
        [SerializeField] private Sprite CompletedStatusBackground;

        public void SetText(string text)
        {
            stepText.text = text;
        }

        public void UpdateStepStatus(QuestStepStatusEnum status)
        {
            switch (status)
            {
                case QuestStepStatusEnum.Finished:
                    statusBackground.sprite = CompletedStatusBackground;
                    stepText.fontStyle = FontStyles.Strikethrough;
                    break;
                default:
                    statusBackground.sprite = incompleteStatusBackground;
                    stepText.fontStyle = FontStyles.Normal;
                    break;
            }
        }
    }
}
