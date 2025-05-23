using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class Selector : Node
    {
        public Selector(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }
        
        public override NodeStatus Evaluate()
        {
            foreach (var node in childNodes)
            {
                var status = node.Evaluate();
                switch (status)
                {
                    case NodeStatus.Running:
                        currentChild = childNodes.IndexOf(node);
                        return NodeStatus.Running;
                    case NodeStatus.Success:
                        Reset();
                        return NodeStatus.Success;
                }
            }
            
            Reset();
            return NodeStatus.Failure;
        }
    }
}