using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewItemList", menuName = "Data/Inventory/Item List", order = 1)]
    public class ItemList : ScriptableObject
    {
        public List<ItemData> itemList = new();

        public void SortItemList()
        {
            itemList = itemList.OrderBy(x => x.ItemId).ToList();
        }
    }
}
