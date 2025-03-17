using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Inventory/Item Details", order = 0)]
    public class ItemData : ScriptableObject
    {
        [Header("Data")]
        public int itemId;
        public string itemName;
        [TextArea] public string itemDescription;
        public Sprite itemSprite;

        // Getter
        public int ItemId => itemId;
        public string ItemName => itemName;
        public string ItemDescription => itemDescription;
        public Sprite ItemSprite => itemSprite;

        // Add this method for testing purposes
        public void SetItemId(int newId) => itemId = newId;
    }
}
