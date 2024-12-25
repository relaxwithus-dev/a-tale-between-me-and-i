using UnityEngine;

namespace ATBMI.NPCs
{
    public abstract class Trees : MonoBehaviour
    {
        protected Node _rootNode;
        
        protected void Start()
        {
            _rootNode = SetupTree();
        }        

        protected void Update()
        {
            _rootNode?.Evaluate();
        }
        
        protected abstract Node SetupTree();
    }
}