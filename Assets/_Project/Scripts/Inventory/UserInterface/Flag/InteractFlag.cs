using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using ATBMI.Enum;
using ATBMI.Data;

namespace ATBMI.Inventory
{
    public class InteractFlag : FlagBase
    {
        #region Internal Fields

        // Interact
        [SerializeField] private InteractFlagStatus flagStatus;
        [SerializeField] [ShowIf("flagStatus", InteractFlagStatus.Item)] protected ItemData itemData;

        [Space]
        [SerializeField] private Image flagIcon;
        [SerializeField] private Button flagButton;

        // Getter
        public Image FlagIcon => flagIcon;
        public Button FlagButton => flagButton;

        #endregion

        #region Methods

        public void SetFlags(int flag, string name, InteractFlagStatus status, ItemData data = null)
        {
            flagId = flag;
            flagName = name;
            itemData = data;
            flagStatus = status;
        }

        public (InteractFlagStatus status, ItemData data) GetItemFlags()
        {
            return (flagStatus, itemData);
        }
        
        #endregion
    }
}