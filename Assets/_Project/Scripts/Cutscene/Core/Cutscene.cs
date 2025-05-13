using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Entities;

namespace ATBMI.Cutscene
{
    public abstract class Cutscene : MonoBehaviour
    {
        #region Global Fields

        protected enum TargetType { Player, Character }
        
        [Header("Global")]
        [SerializeField] protected TargetType type;
        [SerializeField] [ShowIf("type", TargetType.Character)]
        protected Transform targetTransform;
        
        protected IController controller;

        #endregion
        
        #region Methods

        // Unity Callbacks
        private void Start()
        {
            InitOnStart();
        }
        
        // Core
        protected virtual void InitOnStart()
        {
            if (type == TargetType.Player)
                targetTransform = CutsceneManager.Instance.Player;
            
            controller = targetTransform.GetComponent<IController>();
        }
        
        public abstract void Execute();
        public abstract bool IsFinished();
        
        #endregion
    }
}