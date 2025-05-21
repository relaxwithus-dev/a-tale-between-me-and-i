using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Entities;

namespace ATBMI.Cutscene
{
    public abstract class Cutscene : MonoBehaviour
    {
        // Fields
        protected bool isFinishStep;
        
        // Unity Callbacks
        private void Start()
        {
            InitOnStart();
        }
        
        // Core
        protected virtual void InitOnStart() { }
        
        public abstract void Execute();
        public bool IsFinished() => isFinishStep;
    }
}