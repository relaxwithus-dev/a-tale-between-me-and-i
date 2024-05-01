using System;
using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class InteractionBase : MonoBehaviour
    {
        public virtual void Interact() { }
        public virtual void InteractCollectible(CharacterInteract target) { }
    }
}