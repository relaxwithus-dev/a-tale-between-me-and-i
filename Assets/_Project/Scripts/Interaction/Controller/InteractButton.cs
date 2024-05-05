using System;
using System.Collections;
using System.Collections.Generic;
using ATBMI.Enum;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class InteractButton : MonoBehaviour
    {
        #region Fields and Properties

        [Header("Data")]
        [SerializeField] private float buttonId;
        [SerializeField] private InteractType interactType;

        public float ButtonId
        {
            get => buttonId;
            set => buttonId = value;
        }
        public InteractType InteractType => interactType;

        #endregion
    }
}