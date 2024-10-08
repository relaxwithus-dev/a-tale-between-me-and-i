using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Enum;

namespace ATBMI.Interaction
{
    public class Interaction : MonoBehaviour, IInteractable
    {
        #region Fields & Properties

        [Header("General")]
        [SerializeField] private string objectName;
        [SerializeField] [EnumToggleButtons] protected InteractStatus interactStatus;
        
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
        public virtual void Interact(InteractManager manager, int itemId = 0) { }

        #endregion
    }
}