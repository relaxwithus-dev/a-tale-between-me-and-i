using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ATBMI.Enum;

namespace ATBMI.Interaction
{
    [Serializable]
    public struct InteractData
    {
        public int Id;
        public string Description;
        public InteractType Type;
        public Button Button;
        public BaseInteract Interactable;
    }
}