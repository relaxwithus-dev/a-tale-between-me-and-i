using UnityEngine;

namespace ATBMI.Data
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Data/Inventory/Item Details", order = 0)]
    public class ItemData : ScriptableObject
    {
        [Header("Data")]
        [SerializeField] private int itemId;
        [SerializeField] private string itemName;
        [SerializeField] [TextArea] private string itemDescription;
        [SerializeField] private Sprite itemSprite;

        // Getter
        public int ItemId => itemId;
        public string ItemName => itemName;
        public string ItemDescription => itemDescription;
        public Sprite ItemSprite => itemSprite;
    }
}
