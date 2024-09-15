using UnityEngine;
using ATBMI.Enum;
using Sirenix.OdinInspector;

namespace ATBMI.Interaction
{
    public class InteractFlag : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Flags")]
        [SerializeField] private int flagId;
        [SerializeField] private string flagName;
        [SerializeField] private InteractStatus flagStatus;
        [SerializeField] [ShowIf("flagStatus", InteractStatus.Give_Item)] private int itemId;

        #endregion

        #region Methods

        public void SetFlags(int id, string name, InteractStatus status, int item = 0)
        {
            flagId = id;
            flagName = name;
            flagStatus = status;
            itemId = item;
        }

        public (int id, string name, InteractStatus status, int itemId) GetFlags()
        {
            return (flagId, flagName, flagStatus, itemId);
        }

        #endregion
    }
}