using System.Collections.Generic;

namespace ATBMI.NPCs
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }
        
        public override NodeState Evaluate()
        {
            foreach (var child in childrenNode)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Running:
                        currentState = NodeState.Running;
                        return currentState;
                    case NodeState.Success:
                        currentState = NodeState.Success;
                        return currentState;
                    case NodeState.Failure:
                        break;
                }
            }
            
            currentState = NodeState.Failure;
            return currentState;
        }
    }
}
