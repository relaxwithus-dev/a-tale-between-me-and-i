using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class SequenceWeight : Sequence, IEmotionable
    {
        private bool _isInitialized;

        public SequenceWeight(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }

        public override NodeStatus Evaluate()
        {
            if (Validate())
                SetupFactors();
            
            return base.Evaluate();
        }
        
        private bool Validate()
        {
            if (_isInitialized) 
                return false;
            
            _isInitialized = true;
            return true;
        }
        
        private void SetupFactors()
        {
            GetRiskValue();
            GetPlanningValue();
            GetTimeRange();
        }
        
        public float GetRiskValue()
        {
            var totalRisk = 1f;
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable risk)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return 0f;
                }
                totalRisk *= (1 - risk.GetRiskValue());
            }
            return 1 - totalRisk;
        }
        
        public float GetPlanningValue()
        {
            var totalPlanning = 0f;
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable planning)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return 0f;
                }
                totalPlanning += planning.GetPlanningValue();
            }
            return totalPlanning / childNodes.Count;
        }
        
        public (float, float) GetTimeRange()
        {
            var minTime = 0f;
            var maxTime = 0f;
            
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable timeRange)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return (0f, 0f);
                }
                var (L, U) = timeRange.GetTimeRange();
                minTime += L;
                maxTime += U;
            }
            return (minTime, maxTime);
        }
    }
}