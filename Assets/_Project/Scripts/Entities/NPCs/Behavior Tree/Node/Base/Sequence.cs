using System.Collections.Generic;

namespace ATBMI.NPCs
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }
        
        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;
            foreach (var child in childrenNode)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Running:
                        anyChildRunning = true;
                        break;
                    case NodeState.Success:
                        break;
                    case NodeState.Failure:
                        currentState = NodeState.Failure;
                        return currentState;
                }
            }

            currentState = anyChildRunning ? NodeState.Running : NodeState.Success;
            return currentState;
        }
    }
}
