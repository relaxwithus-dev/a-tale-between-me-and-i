using Sirenix.OdinInspector;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public abstract class Trees : MonoBehaviour
    {
        #region Fields & Properties
        
        private Node _rootNode;
        protected string rootName;
        protected CharacterAI characterAI;

        #endregion

        #region Methods
        
        // Unity Callbacks
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
            if (_rootNode != null)
                _rootNode.Evaluate();
        }
        
        // Core
        protected abstract Node SetupTree();
        protected virtual void InitOnAwake()
        {
            characterAI = GetComponentInParent<CharacterAI>();
        }
        
        protected virtual void InitOnStart()
        {
            _rootNode = SetupTree();
            rootName = characterAI.Data.CharacterName;
        }

        #endregion
    }
}