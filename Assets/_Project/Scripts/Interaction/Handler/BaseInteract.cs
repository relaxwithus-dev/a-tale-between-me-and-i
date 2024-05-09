using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class BaseInteract : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Data")]
        [SerializeField] private int interactId;
        [SerializeField] private TextAsset dialogueAsset;

        public int InteractId => interactId;
        
        #endregion

        public virtual void Interact() { }
        public virtual void InteractCollectible(BaseInteract target) { }
    }
}