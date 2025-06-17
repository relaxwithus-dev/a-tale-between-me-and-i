using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace ATBMI.Quest
{
    public class QuestLogButton : MonoBehaviour
    {
        public int QuestId { get; private set; }  // Store the quest's ID

        [SerializeField] private Image background;
        [SerializeField] private Image selectedIcon;
        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private GameObject checkSprite;
        [SerializeField] private Sprite deselectedStatusBackground;
        [SerializeField] private Sprite selectedStatusBackground;

        public void Initialize(string displayName, int questId)
        {
            buttonText.text = displayName;
            QuestId = questId;
        }

        public void SetState(QuestStateEnum state)
        {
            if (state == QuestStateEnum.Finished)
            {
                checkSprite.SetActive(true);
            }
            else
            {
                checkSprite.SetActive(false);
            }
        }

        public void Highlight(bool isSelected)
        {
            // TODO: change the highligh selected
            background.sprite = isSelected ? selectedStatusBackground : deselectedStatusBackground; // Change color
            selectedIcon.gameObject.SetActive(isSelected);
        }
    }
}
