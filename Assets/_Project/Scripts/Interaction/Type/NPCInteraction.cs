using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;
using ATBMI.Data;
using ATBMI.Gameplay.Event;

namespace ATBMI.Interaction
{
    public class NPCInteraction : Interaction
    {
        #region Fields & Properties

        [Header("NPC")]
        [SerializeField] private TextAsset[] dialogueAssets;
        [SerializeField] [ShowIf("interactStatus", InteractStatus.Take_Item)] private int targetId;
        [SerializeField] [ShowIf("interactStatus", InteractStatus.Give_Item)] private ItemData itemData;

        private int _interactedId;
        public ItemData ItemData => itemData;

        #endregion

        #region Methods

        // TODO: Drop method call dialogue, sesuai jenis interaksi
        public override void Interact(InteractManager manager, int itemId = 0)
        {
            base.Interact(manager, itemId);

            _interactedId = itemId;
            switch (interactStatus)
            {
                case InteractStatus.Talks:
                    Debug.Log("hi bro, jangan lupa #timnasday");
                    break;
                case InteractStatus.Take_Item:
                    Debug.Log("oke aku ambil itemnya ya!");
                    break;
                case InteractStatus.Give_Item:
                    Debug.Log("ini kuberikan item padamu");
                    break;
            }
            
            // TODO: change this
            DialogEvents.EnterDialogueEvent();
        }
        
        // TODO: Pake ini buat change status di InkExternal
        public void ChangeStatus(InteractStatus status)
        {
            if (interactStatus == status) return;
            interactStatus = status;
        }

        // TODO: Pake ini buat check match id di InkExternal
        public bool IsMatchId()
        {
            return _interactedId == targetId;
        }


        #endregion
    }
}