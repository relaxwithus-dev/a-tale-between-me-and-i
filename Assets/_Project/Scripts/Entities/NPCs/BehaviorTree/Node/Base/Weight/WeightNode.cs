using System.Collections.Generic;

namespace ATBMI.Entities.NPCs
{
    public class WeightNode : Node
    {
        // Parameters
        protected float planning;
        protected float time;
        protected float risk;
        
        public WeightNode(string nodeName, List<Node> childNodes) : base(nodeName, childNodes)
        {
            
        }
        
        public float GetPlanning() => planning;
        public float GetTime() => time;
        public float GetRisk() => risk;
    }
}