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

        #endregion

        #region Methods

        public void SetFlags(int flag, string name, ItemData data)
        {
            flagId = flag;
            flagName = name;
            itemData = data;
        }
        
        #endregion
    }
}