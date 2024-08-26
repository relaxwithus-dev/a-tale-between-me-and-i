using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class SceneryInteraction : Interaction
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField] private string itemName;
        
        #endregion

        #region Methods

        public override void Interact(InteractManager manager)
        {
            base.Interact(manager);
            throw new NotImplementedException();
        }

        #endregion
    }
}