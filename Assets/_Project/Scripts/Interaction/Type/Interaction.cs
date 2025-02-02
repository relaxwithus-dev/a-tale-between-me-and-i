using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;

namespace ATBMI.Interaction
{
    public class Interaction : MonoBehaviour, IInteractable
    {
        #region Fields & Properties
        
        [Header("General")]
        [SerializeField] protected bool isInteracting;
        
        public bool IsInteracting
        {
            get => isInteracting; 
            set => isInteracting = value;
        }
        
        #endregion
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            InitOnStart();
        }

        #endregion
        
        #region Methods
        
        protected virtual void InitOnStart() { }
        public virtual void Interact(InteractManager manager, int itemId = 0)
        {
            StartInteract();
        }
        
        public void StartInteract() => isInteracting = true;
        public void StopInteract() => isInteracting = false;
        
        #endregion
    }
}