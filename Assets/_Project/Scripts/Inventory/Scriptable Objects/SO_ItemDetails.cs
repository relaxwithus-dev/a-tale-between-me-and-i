using UnityEngine;

namespace ATBMI.Inventory
{
    [CreateAssetMenu(fileName = "NewItemDetails", menuName = "Data/Inventory/Item Details")]
    public class SO_ItemDetails : ScriptableObject
    {
        public int itemId;
        public string itemName;
        public bool isStartingItem;
        public Sprite itemSprite;
        [TextArea] public string itemDescription;
    }
}
