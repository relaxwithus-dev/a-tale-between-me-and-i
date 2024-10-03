using UnityEngine;
using ATBMI.Enum;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace ATBMI.Interaction
{
    public class InteractFlag : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Flags")]
        [SerializeField] private int flagId;
        [SerializeField] private string flagName;
        [SerializeField] private InteractFlagStatus flagStatus;
        [SerializeField] [ShowIf("flagStatus", InteractFlagStatus.Item)] private int itemId;

        [Space]
        [SerializeField] private Image flagIcon;
        [SerializeField] private Button flagButton;

        // Getter
        public int FlagId => flagId;
        public string FlagName => flagName;
        public Image FlagIcon => flagIcon;
        public Button FlagButton => flagButton;

        #endregion

        #region Methods

        public void SetFlags(int flag, string name, InteractFlagStatus status, int item = 0)
        {
            flagId = flag;
            itemId = item;
            flagName = name;
            flagStatus = status;
        }

        public (InteractFlagStatus status, int itemId) GetItemFlags()
        {
            return (flagStatus, itemId);
        }
        
        #endregion
    }
}