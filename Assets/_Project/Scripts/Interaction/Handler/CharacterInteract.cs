using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class CharacterInteract : BaseInteract
    {
        public RuleEntry ruleEntry;

        #region Methods

        // TODO: Drop method for dialogue here
        public override void Interact()
        {
            base.Interact();
            ruleEntry.EnterDialogue();
            Debug.Log("kOngCap Lai Mewe WinDow kOsLay TiitIT LayYa!");
        }

        #endregion
    }
}