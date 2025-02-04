using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class EmotionalSelector : Node
    {
        private readonly List<WeightNode> _weightNodes = new();
        
        public EmotionalSelector(string nodeName, List<Node> childNodes) : base(nodeName, childNodes)
        {
            foreach (var child in childNodes)
            {
                if (child is WeightNode node)
                {
                    _weightNodes.Add(node);
                }
            }
        }

        public override NodeStatus Evaluate()
        {
            return base.Evaluate();
        }
    }
}