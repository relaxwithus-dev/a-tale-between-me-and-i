using System;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;

namespace ATBMI.Interaction
{
    public class NPCInteraction : Interaction
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private string characterName;
        [SerializeField] [EnumToggleButtons] private ItemAccess itemAccess;
        [SerializeField] [ShowIf("itemAccess", ItemAccess.Receive)] private int itemTargetId;
        [SerializeField] [ShowIf("itemAccess", ItemAccess.Give)] private GameObject itemPrefabs;

        #endregion

        #region Methods

        public override void Interact(InteractManager manager, InteractStatus status)
        {
            base.Interact(manager, status);
            switch (status)
            {
                case InteractStatus.Talks:
                    Debug.Log("hi bro, jangan lupa #timnasday");
                    break;
                case InteractStatus.Take_Item:
                    if (itemAccess == ItemAccess.Give)
                        Debug.Log("ini kuberikan item padamu");
                    else
                        Debug.Log("tidak terjadi apa-apa");
                    break;
                case InteractStatus.Give_Item:
                    if (manager.ItemId == itemTargetId)
                        Debug.Log("oke aku ambil itemnya ya!");
                    else
                        Debug.Log("tidak terjadi apa-apa");
                    break;
            }
        }
        
        #endregion
    }
}