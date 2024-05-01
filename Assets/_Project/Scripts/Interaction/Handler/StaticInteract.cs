using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class StaticInteract : InteractionBase
    {
        #region Fields & Property
        
        [Header("Data")]
        [SerializeField] private int itemId;
        [SerializeField] private TextAsset dialogueAsset;

        #endregion

        #region Methods

        public override void Interact()
        {
            base.Interact();
            // TODO: Drop method for dialogue here
            Debug.Log("kOngCap Lai Mewe WinDow kOsLay FittiT LayYa!");
        }

        #endregion
    }
}