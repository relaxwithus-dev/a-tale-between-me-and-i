using UnityEngine;

namespace ATBMI.Cutscene
{
    public abstract class Cutscene : MonoBehaviour
    {
        // Unity Callbacks
        private void Awake()
        {
            InitOnAwake();
        }

        private void Start()
        {
            InitOnStart();
        }

        // Core
        protected virtual void InitOnAwake() { }
        protected virtual void InitOnStart() { }
        
        public abstract void Execute();
        public abstract bool IsFinished();
    }
}