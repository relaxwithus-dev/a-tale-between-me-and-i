using UnityEngine;
using ATBMI.Enum;

namespace ATBMI.Interaction
{
    public class ItemInteraction : Interaction
    {
         #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private int itemId;
        [SerializeField] private string itemName;

        #endregion

        #region Methods

        public override void Interact(InteractManager manager, InteractStatus status)
        {
            base.Interact(manager, status);
        }

        #endregion
    }
}