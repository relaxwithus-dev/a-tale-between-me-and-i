using ATBMI.Gameplay.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ATBMI
{
    public class QuestStepLog : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stepText;
        [SerializeField] private Image statusIcon; //  To visually indicate step completion

        public void SetText(string text)
        {
            stepText.text = text;
        }

        public void UpdateStepStatus(QuestStepStatusEnum status)
        {
            switch (status)
            {
                case QuestStepStatusEnum.Null:
                    // statusIcon.color = Color.white;
                    stepText.fontStyle = TMPro.FontStyles.Normal;
                    break;
                case QuestStepStatusEnum.In_Progress:
                    // statusIcon.color = Color.white;
                    stepText.fontStyle = TMPro.FontStyles.Normal;
                    break;
                case QuestStepStatusEnum.Finished:
                    // statusIcon.color = Color.green;
                    stepText.fontStyle = TMPro.FontStyles.Strikethrough;
                    break;
            }
        }
    }
}
