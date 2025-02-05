using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class EmotionalSelector : Node
    {
        public EmotionalSelector(string nodeName, List<Node> childNodes) : base(nodeName, childNodes)
        {
            
        }
        
        public override NodeStatus Evaluate()
        {
            return base.Evaluate();
        }
    }
}