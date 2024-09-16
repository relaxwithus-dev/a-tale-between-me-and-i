using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ATBMI.Inventory
{
    [CreateAssetMenu(fileName = "NewItemList", menuName = "Data/Inventory/Item List")]
    public class SO_ItemList : ScriptableObject
    {
        public List<SO_ItemDetails> itemList = new List<SO_ItemDetails>();

        // This is a button, draws from ItemListEditor Helper script
        public void SortItemList()
        {
            itemList = itemList.OrderBy(x => x.itemId).ToList();
            Debug.Log("Item list sorted by itemId.");
        }
    }
}
