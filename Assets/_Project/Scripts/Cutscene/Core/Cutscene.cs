using UnityEngine;

namespace ATBMI.Cutscene
{
    public abstract class Cutscene : MonoBehaviour
    {
        #region Methods

        // Unity Callbacks
        private void Start()
        {
            InitOnStart();
        }
        
        // Core
        protected virtual void InitOnStart() { }
        
        public abstract void Execute();
        public abstract bool IsFinished();
        
        #endregion
    }
}