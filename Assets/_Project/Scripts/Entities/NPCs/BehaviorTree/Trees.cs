using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public abstract class Trees : MonoBehaviour
    {
        protected Node rootNode;

        private void Awake()
        {
            InitOnAwake();
        }

        private void Start()
        {
            InitOnStart();
            rootNode = SetupTree();
        }

        private void Update()
        {
            if (rootNode != null)
                rootNode.Evaluate();
        }
        
        protected virtual void InitOnAwake() { }
        protected virtual void InitOnStart() { }
        protected abstract Node SetupTree();
    }
}