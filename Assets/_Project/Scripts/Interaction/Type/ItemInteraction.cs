using ATBMI.Inventory;
using ATBMI.Item;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class ItemInteraction : Interaction
    {
         #region Fields & Properties

        [Header("Item")]
        [SerializeField] private TextAsset dialogueAsset;
        [SerializeField] private ItemController itemPrefabs;
        [SerializeField] private InventoryManager inventory;

        #endregion

        #region Methods

        public override void Interact(InteractManager manager, int status)
        {
            base.Interact(manager, status);
            statusSucces = true;
            Debug.Log("bagus jg ni barang");
            inventory.Item.Add(itemPrefabs);
            Destroy(gameObject);
        }

        #endregion
    }
}