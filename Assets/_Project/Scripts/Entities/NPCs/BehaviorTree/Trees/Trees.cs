using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [RequireComponent(typeof(CharacterAI))]
    public abstract class Trees : MonoBehaviour
    {
        private Node _rootNode;
        protected string rootName;
        protected CharacterAI characterAI;
        
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
        
        protected abstract Node SetupTree();
        protected virtual void InitOnAwake()
        {
            characterAI = GetComponent<CharacterAI>();
        }

        protected virtual void InitOnStart()
        {
            _rootNode = SetupTree();
            rootName = characterAI.Data.CharacterName;
        }
    }
}