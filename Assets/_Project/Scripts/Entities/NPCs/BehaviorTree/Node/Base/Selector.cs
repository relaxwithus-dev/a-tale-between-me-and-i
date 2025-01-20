using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class Selector : Node
    {
        public Selector(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }

        public override NodeStatus Evaluate()
        {
            if (currentChild < childNodes.Count)
            {
                var status = childNodes[currentChild].Evaluate();
                switch (status)
                {
                    case NodeStatus.Running:
                        return NodeStatus.Running;
                    case NodeStatus.Success:
                        Reset();
                        return NodeStatus.Success;
                    default:
                        currentChild++;
                        return NodeStatus.Running;
                }
            }
            
            Reset();
            return NodeStatus.Failure;
        }
    }
}