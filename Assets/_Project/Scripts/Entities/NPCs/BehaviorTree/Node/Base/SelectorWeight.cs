using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class SelectorWeight : Selector, IEmotionable
    {
        private bool _isInitialized;

        public SelectorWeight(string nodeName, List<Node> childNodes) : base(nodeName, childNodes) { }

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
            var totalRisk = 0f;
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable risk)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return 0f;
                }
                totalRisk += risk.GetRiskValue();
            }
            return totalRisk / childNodes.Count;
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
            var minTime = float.MaxValue;
            var maxTime = float.MinValue;
            
            foreach (var child in childNodes)
            {
                if (child is not IEmotionable timeRange)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return (0f, 0f);
                }
                var (L, U) = timeRange.GetTimeRange();
                minTime = Mathf.Min(minTime, L);
                maxTime = Mathf.Max(maxTime, U);
            }
            return (minTime, maxTime);
        }
    }
}