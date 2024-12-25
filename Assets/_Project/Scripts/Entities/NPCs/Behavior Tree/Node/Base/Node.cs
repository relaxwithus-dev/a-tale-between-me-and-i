using System;
using System.Collections.Generic;

namespace ATBMI.NPCs
{
    [Serializable]
    public abstract class Node
    {
        protected Node parentNode;
        public Node ParentNode => parentNode;
        
        protected NodeState currentState;
        protected List<Node> childrenNode = new();
        
        private readonly Dictionary<string, object> _dataContext = new();


        // !- Constructor
        public Node()
        {
            parentNode = null;
        }
        
        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                child.parentNode = this;
                childrenNode.Add(child);
            }
        }

        // !- Core
        public abstract NodeState Evaluate();
        
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }
        
        public object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out object value))
                return value;

            var node = parentNode;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;

                node = node.parentNode;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            var node = parentNode;
            while (node != null)
            {
                var cleared = node.ClearData(key);
                if (cleared)
                    return true;
                
                node = node.parentNode;
            }

            return false;
        }
    }
}
