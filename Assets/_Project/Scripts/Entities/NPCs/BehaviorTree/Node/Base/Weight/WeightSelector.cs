using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class WeightSelector : WeightNode
    {
        public WeightSelector(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }
        
        public override NodeStatus Evaluate()
        {
            if (Validate())
                SetupFactors();
            
            if (currentChild < childWeights.Count)
            {
                var status = childWeights[currentChild].Evaluate();
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
        
        private void SetupFactors()
        {
            GetRiskValue();
            GetPlanningValue();
            GetTimeRange();
        }
        
        public override float GetRiskValue()
        {
            var totalRisk = 0f;
            foreach (var child in childWeights)
            {
                totalRisk += child.GetRiskValue();
            }
            return totalRisk / childWeights.Count;
        }

        public override float GetPlanningValue()
        {
            var totalPlanning = 0f;
            foreach (var child in childWeights)
            {
                totalPlanning += child.GetRiskValue();
            }
            return totalPlanning / childWeights.Count;
        }
        
        public override (float, float) GetTimeRange()
        {
            var minTime = float.MaxValue;
            var maxTime = float.MinValue;
            
            foreach (var child in childWeights)
            {
                var (L, U) = child.GetTimeRange();
                minTime = Mathf.Min(minTime, L);
                maxTime = Mathf.Max(maxTime, U);
            }
            return (minTime, maxTime);
        }
    }
}