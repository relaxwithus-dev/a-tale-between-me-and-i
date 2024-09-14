using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Item
{
    public class ItemController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Data")]
        [SerializeField] private string itemName;
        [SerializeField] private int itemId;
        
        // Reference
        private SpriteRenderer _itemSr;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _itemSr = GetComponentInChildren<SpriteRenderer>();
        }

        #endregion

        #region Methods

        // Getter
        public (string name, int id, Sprite sprite) GetItemData()
        {
            (string name, int id, Sprite sprite) tempData;
            if (itemName != null || itemId != 0 || _itemSr.sprite != null)
            {
                tempData = (itemName, itemId, _itemSr.sprite);
                return tempData;
            }

            tempData = ("null name", 0, null);
            return tempData;
        }

        #endregion
    }
}
