using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;
using ATBMI.Item;
using ATBMI.Inventory;

namespace ATBMI.Interaction
{
    public class NPCInteraction : Interaction
    {
        #region Fields & Properties

        [Header("NPC")]
        [SerializeField] private TextAsset dialogueAsset;
        [SerializeField] [ShowIf("interactStatus", InteractStatus.Take_Item)] private int itemTargetId;
        [SerializeField] [ShowIf("interactStatus", InteractStatus.Give_Item)] private ItemController itemPrefabs;

        [Header("Reference")]
        [SerializeField] private InventoryManager inventory;

        #endregion

        #region Methods

        public override void Interact(InteractManager manager, int itemId)
        {
            base.Interact(manager, itemId);
            switch (interactStatus)
            {
                case InteractStatus.Talks:
                    statusSucces = true;
                    Debug.Log("hi bro, jangan lupa #timnasday");
                    break;
                case InteractStatus.Take_Item:
                    statusSucces = itemId == itemTargetId;
                    if (statusSucces)
                    {
                        // TODO: Drop mekanik remove item d inventory
                        Debug.Log("oke aku ambil itemnya ya!");
                        interactStatus = InteractStatus.Talks;
                    }
                    else
                    {
                        Debug.Log("tidak terjadi apa-apa");
                    }
                    break;
                case InteractStatus.Give_Item:
                    statusSucces = itemPrefabs != null;
                    if (statusSucces)
                    {
                        // TODO: Drop mekanik add item d inventory
                        Debug.Log("ini kuberikan item padamu");
                        inventory.Item.Add(itemPrefabs);
                        interactStatus = InteractStatus.Talks;
                    }
                    else
                    {
                        Debug.Log("tidak terjadi apa-apa");
                    }
                    break;
            }
        }
        
        #endregion
    }
}