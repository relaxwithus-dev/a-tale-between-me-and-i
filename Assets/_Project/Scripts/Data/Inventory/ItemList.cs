using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewItemList", menuName = "Data/Inventory/Item List", order = 1)]
    public class ItemList : ScriptableObject
    {
        public List<ItemData> itemList = new();

        // This is a button, draws from ItemListEditor Helper script
        public void SortItemList()
        {
            itemList = itemList.OrderBy(x => x.ItemId).ToList();
            Debug.Log("Item list sorted by itemId.");
        }
    }
}
