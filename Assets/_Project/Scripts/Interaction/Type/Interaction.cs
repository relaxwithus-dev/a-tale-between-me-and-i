using System;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class Interaction : MonoBehaviour, IInteractable
    {
        #region Fields & Properties

        #endregion

        #region MonoBehaviour Callbacks

        #endregion

        #region Methods

        protected virtual void InitOnAwake() { }
        protected virtual void InitOnStart() { }
        protected virtual void HandleUpdate() { }

        public virtual void Interact(InteractManager manager)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}