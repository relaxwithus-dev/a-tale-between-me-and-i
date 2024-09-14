using System;
using ATBMI.Enum;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class Interaction : MonoBehaviour, IInteractable
    {
        #region Fields & Properties

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            InitOnAwake();
        }

        private void Start()
        {
            InitOnStart();
        }

        private void Update()
        {
            HandleUpdate();
        }

        #endregion

        #region Methods

        protected virtual void InitOnAwake() { }
        protected virtual void InitOnStart() { }
        protected virtual void HandleUpdate() { }

        public virtual void Interact(InteractManager manager, InteractStatus status) { }

        #endregion
    }
}