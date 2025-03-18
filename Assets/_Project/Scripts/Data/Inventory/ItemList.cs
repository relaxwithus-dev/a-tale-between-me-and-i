using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewItemList", menuName = "Data/Inventory/Item List", order = 1)]
    public class ItemList : ScriptableObject
    {
        public List<ItemData> itemList = new();

        // NOTE: still need to save the project after changing stuffs
        private void OnValidate()
        {
#if UNITY_EDITOR
            itemList = itemList.OrderBy(x => x.ItemId).ToList();
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
