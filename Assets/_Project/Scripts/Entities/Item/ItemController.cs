using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Item
{
    public class ItemController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Data")]
        [SerializeField] private int itemId;
        [SerializeField] private string itemName;
        
        // Reference
        [SerializeField] private SpriteRenderer itemSr;

        #endregion

        #region Methods

        // Getter
        public (string name, int id, Sprite sprite) GetItemData()
        {
            (string name, int id, Sprite sprite) tempData;
            if (itemName != null || itemId != 0 || itemSr.sprite != null)
            {
                tempData = (itemName, itemId, itemSr.sprite);
                return tempData;
            }

            tempData = (itemName, itemId, itemSr.sprite);
            return tempData;
        }

        #endregion
    }
}
