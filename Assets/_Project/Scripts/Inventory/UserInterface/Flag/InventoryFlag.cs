using UnityEngine;
using UnityEngine.UI;
using ATBMI.Data;
using TMPro;

namespace ATBMI.Inventory
{
    public class InventoryFlag : FlagBase
    {
        #region Internal Fields

        // Inventory
        [SerializeField] protected ItemData itemData;


        [Space]
        [SerializeField] private Button flagButton;
        [SerializeField] private Image flagImage;
        [SerializeField] private TextMeshProUGUI flagNameText;

        public ItemData ItemData => itemData;
        public Button FlagButton => flagButton;
        public Image FlagImage => flagImage;
        public TextMeshProUGUI FlagNameText => flagNameText;

        [Space(20)]
        [Header("References")]
        [SerializeField] private Image buttonBackgroundImage;
        [SerializeField] private Image itemBackgroundImage;

        [Header("Button Background Sprite")]
        [SerializeField] private Sprite buttonBGSpriteSelected;
        [SerializeField] private Sprite buttonBGSpriteDeselected;

        [Header("Item Background Sprite")]
        [SerializeField] private Sprite itemBGSpriteSelected;
        [SerializeField] private Sprite itemBGSpriteDeselected;

        [Header("Text Color")]
        [SerializeField] private Color textColorSelected;
        [SerializeField] private Color textColorDeslected;

        #endregion

        #region Methods

        public void SetFlags(int flag, string name, ItemData data)
        {
            flagId = flag;
            flagName = name;
            itemData = data;
        }

        public void Highlight(bool isSelected)
        {
            // TODO: change the highligh selected
            if (isSelected)
            {
                buttonBackgroundImage.sprite = buttonBGSpriteSelected;
                itemBackgroundImage.sprite = itemBGSpriteSelected;

                flagNameText.color = textColorSelected;
            }
            else
            {
                buttonBackgroundImage.sprite = buttonBGSpriteDeselected;
                itemBackgroundImage.sprite = itemBGSpriteDeselected;

                flagNameText.color = textColorDeslected;
            }
        }

        #endregion
    }
}