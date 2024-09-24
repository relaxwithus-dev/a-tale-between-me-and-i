using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class StaticInteract : BaseInteract
    {
        #region Methods

        // TODO: Drop method for dialogue here
        public override void Interact()
        {
            base.Interact();
            Debug.Log("aku static item lekk, gabisa diambil wokwok");
        }

        #endregion
    }
}