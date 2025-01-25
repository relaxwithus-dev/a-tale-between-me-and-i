using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class Sequence : Node
    {
        public Sequence(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }

        public override NodeStatus Evaluate()
        {
            if (currentChild < childNodes.Count)
            {
                var status = childNodes[currentChild].Evaluate();
                switch (status)
                {
                    case NodeStatus.Running:
                        return NodeStatus.Running;
                    case NodeStatus.Failure:
                        Reset();
                        return NodeStatus.Failure;
                    default:
                        currentChild++;
                        return currentChild == childNodes.Count ? NodeStatus.Success : NodeStatus.Running;
                }
            }
            
            Reset();
            return NodeStatus.Success;
        }
    }
}