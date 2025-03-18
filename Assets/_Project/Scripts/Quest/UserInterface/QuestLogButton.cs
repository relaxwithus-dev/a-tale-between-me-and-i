using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ATBMI
{
    public class QuestLogButton : MonoBehaviour
    {
        public int QuestId { get; private set; }  // Store the quest's ID

        [SerializeField] private Image background;
        [SerializeField] private Image check;
        [SerializeField] private TextMeshProUGUI buttonText;

        public void Initialize(string displayName, int questId)
        {
            this.buttonText.text = displayName;
            this.QuestId = questId;
        }

        public void SetState(QuestStateEnum state)
        {
            switch (state)
            {
                case QuestStateEnum.Can_Start:
                    check.color = Color.red;
                    break;
                case QuestStateEnum.In_Progress:
                case QuestStateEnum.Can_Finish:
                    check.color = Color.yellow;
                    break;
                case QuestStateEnum.Finished:
                    check.color = Color.green;
                    break;
                default:
                    Debug.LogWarning("Quest State not recognized by switch statement: " + state);
                    break;
            }
        }

        public void Highlight(bool isSelected)
        {
            // TODO: change the highligh selected
            background.color = isSelected ? Color.yellow : Color.white; // Change color
        }
    }
}
