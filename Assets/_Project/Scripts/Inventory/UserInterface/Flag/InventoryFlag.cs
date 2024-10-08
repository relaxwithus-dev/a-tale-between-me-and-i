using UnityEngine;
using UnityEngine.UI;
using ATBMI.Data;

namespace ATBMI.Inventory
{
    public class InventoryFlag : FlagBase
    {
        #region Internal Fields

        // Inventory
        [SerializeField] protected ItemData itemData;

        [Space]
        [SerializeField] private Image flagImage;

        public ItemData ItemData => itemData;
        public Image FlagImage => flagImage;

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