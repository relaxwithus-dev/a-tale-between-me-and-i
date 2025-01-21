using System;
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
        private SpriteRenderer _itemSr;

        #endregion

        #region Methods

        private void Awake()
        {
            _itemSr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            gameObject.name = itemName;
        }

        // Getter
        public (string name, int id, Sprite sprite) GetItemData()
        {
            (string name, int id, Sprite sprite) tempData;
            if (itemName != null || itemId != 0 || _itemSr.sprite != null)
            {
                tempData = (itemName, itemId, _itemSr.sprite);
                return tempData;
            }

            tempData = (itemName, itemId, _itemSr.sprite);
            return tempData;
        }

        #endregion
    }
}
