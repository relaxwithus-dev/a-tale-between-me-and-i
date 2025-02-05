using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class WeightSequence : WeightNode
    {
        public WeightSequence(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }
        
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
        
        private void SetupFactors()
        {
            GetRiskValue();
            GetPlanningValue();
            GetTimeRange();
        }
        
        public override float GetRiskValue()
        {
            var totalRisk = 1f;
            foreach (var child in childWeights)
            {
                totalRisk *= (1 - child.GetRiskValue());
            }
            return 1 - totalRisk;
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
            var minTime = 0f;
            var maxTime = 0f;
            
            foreach (var child in childWeights)
            {
                var (L, U) = child.GetTimeRange();
                minTime += L;
                maxTime += U;
            }
            return (minTime, maxTime);
        }
    }
}