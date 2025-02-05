using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class WeightNode : Node
    {
        protected readonly List<WeightNode> childWeights = new();
        private bool _isInitialized;

        public WeightNode() : base() { }
        public WeightNode(string nodeName, List<Node> childNodes) : base(nodeName, childNodes)
        {
            foreach (var child in childNodes)
            {
                if (child is WeightNode node)
                {
                    childWeights.Add(node);
                }
            }
        }
        
        public virtual float GetRiskValue() { return 0f; }
        public virtual float GetPlanningValue() { return 0f; }
        public virtual (float, float)GetTimeRange() { return (0f, 0f); }
        
        protected bool Validate()
        {
            if (_isInitialized) 
                return false;
            
            _isInitialized = true;
            return true;
        }
    }
}