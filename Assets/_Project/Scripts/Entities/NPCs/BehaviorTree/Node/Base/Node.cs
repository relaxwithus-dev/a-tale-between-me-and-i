using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class Node
    {
        #region Base Fields
        
        protected Node parentNode;
        public readonly string nodeName;
        protected readonly List<Node> childNodes = new();
        protected int currentChild;
        
        private readonly Dictionary<string, object> _dataContext = new();
        
        // Cached properties
        protected const string TARGET_KEY = "Target";
        protected const string ORIGIN_KEY = "Origin";
        protected const string PHYSIC_KEY = "Physics";

        #endregion
        
        #region Methods
        
        public Node()
        {
            parentNode = null;
        }
        
        public Node(string nodeName)
        {
            this.nodeName = nodeName;
        }
        
        public Node(string nodeName, List<Node> childNodes)
        {
            this.nodeName = nodeName;
            foreach (var node in childNodes)
            {
                this.childNodes.Add(node);
                node.parentNode = this;
            }
        }
        
        public virtual NodeStatus Evaluate()
        {
            return NodeStatus.Failure;
        }
        
        protected virtual void Reset()
        {
            currentChild = 0;
            foreach (var child in childNodes)
                child.Reset();
        }
        
        // Data context
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }
        
        public object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
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
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parentNode;
            }
            return false;
        }
        
        #endregion
        
    }
}