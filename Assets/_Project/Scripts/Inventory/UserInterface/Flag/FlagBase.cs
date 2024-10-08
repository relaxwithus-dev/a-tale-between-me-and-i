using ATBMI.Data;
using UnityEngine;

namespace ATBMI.Inventory
{
    public class FlagBase : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Flags")]
        [SerializeField] protected int flagId;
        [SerializeField] protected string flagName;

        // Getter
        public int FlagId => flagId;
        public string FlagName => flagName;

        #endregion
    }
}