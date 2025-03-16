using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

namespace ATBMI.Entities.NPCs
{
    public class EmotionalSelector : Node
    {
        // Fields
        private enum Factors { Plan, Risk, Time }
        private readonly CharacterTraits traits;
        
        private Node _selectedNode;
        
        // Cached fields
        private readonly float delta = 0.9f;   // Risk impact
        private readonly float lambda = 0.9f;  // Time discounting impact
        private readonly float phi = 0.8f;     // Time discounting weight
        private readonly float omega = 0.6f;   // Planning weight
        private readonly float sign = 0.6f;    // Planning impact
        private readonly float sigma = 0.5f;   // Time adjustable

        private readonly float opt = 1f;      // Time factor optimizer
        private readonly float bias = 0.6f;   // Adjustable probability
        private readonly float alpha = 1.0f;  // Importance of risk
        private readonly float beta = 1.0f;   // Importance of time
        private readonly float gamma = 1.0f;  // Importance of planning
        
        private readonly Dictionary<Emotion, Dictionary<Factors, float>> emotionModifiers = new()
        {
            { Emotion.Joy, new Dictionary<Factors, float> { { Factors.Plan, 1 }, { Factors.Risk, 1 }, { Factors.Time, 1 } } },
            { Emotion.Trust, new Dictionary<Factors, float> { { Factors.Plan, 0 }, { Factors.Risk, 1 }, { Factors.Time, 1 } } },
            { Emotion.Fear, new Dictionary<Factors, float> { { Factors.Plan, 0 }, { Factors.Risk, 0 }, { Factors.Time, 0 } } },
            { Emotion.Surprise, new Dictionary<Factors, float> { { Factors.Plan, 0 }, { Factors.Risk, 1 }, { Factors.Time, 0 } } },
            { Emotion.Sadness, new Dictionary<Factors, float> { { Factors.Plan, 0 }, { Factors.Risk, 0 }, { Factors.Time, 1 } } },
            { Emotion.Disgust, new Dictionary<Factors, float> { { Factors.Plan, 0 }, { Factors.Risk, 0 }, { Factors.Time, 1 } } },
            { Emotion.Anger, new Dictionary<Factors, float> { { Factors.Plan, 1 }, { Factors.Risk, 1 }, { Factors.Time, 0 } } },
            { Emotion.Anticipation, new Dictionary<Factors, float> { { Factors.Plan, 1 }, { Factors.Risk, 0 }, { Factors.Time, 1 } } }
        };
        
        // Constructor
        public EmotionalSelector(string nodeName, CharacterTraits traits, List<Node> childNodes) : base(nodeName, childNodes)
        {
            this.traits = traits;
        }
        
        // Core
        public override NodeStatus Evaluate()
        {
            _selectedNode ??= SelectNodeAction();
            return _selectedNode.Evaluate();
        }
        
        protected override void Reset()
        {
            base.Reset();
            _selectedNode = null;
        }
        
        private Node SelectNodeAction()
        {
            // Calculate emotional factor
            var (emotion, intensity) = traits.GetDominantEmotion();
            
            var emoPlan = CalculateEmotionalFactor(Factors.Plan) - emotionModifiers[emotion][Factors.Plan];
            var emoRisk = CalculateEmotionalFactor(Factors.Risk) - emotionModifiers[emotion][Factors.Risk];
            var emoTime = CalculateEmotionalFactor(Factors.Time) - emotionModifiers[emotion][Factors.Time];
            
            // Compute weighted values for each child node
            var weightedNodes = new Dictionary<Node, float>();
            foreach (var child in childNodes)
            {
                float weight = CalculateWeight(child, emoPlan, emoRisk, emoTime);
                weightedNodes[child] = weight;
            }
            
            // Sort actions by weight (lower = better)
            var sortedNodes = weightedNodes.OrderBy(kv => kv.Value).ToList();
            return SelectActionWithProbability(sortedNodes);
        }
        
        private float CalculateWeight(Node node, float emoPlan, float emoRisk, float emoTime)
        {
            if (node is not IEmotionable emoChild)
            {
                Debug.LogError($"{node.nodeName} is not an IEmotionable!");
                return 0f;
            }
            
            // Raw node value
            var risk = emoChild.GetRiskValue();
            var planning = emoChild.GetPlanningValue();
            (float U, float L) timeRange = emoChild.GetTimeRange();
            var time = (timeRange.L + (timeRange.U - timeRange.L) / 2) * (1 - sigma * opt);
            
            // Weight calculated
            var weightRisk = Mathf.Clamp((1 - emoRisk * delta) * risk, 0, 1);
            var weightPlan = (1 - 1 / (1 + omega * planning)) * Mathf.Max(1 - sign + sign * emoPlan, 0);
            var weightTime = (1 - 1 / (1 + phi * time)) * Mathf.Max(1 - lambda + lambda * emoTime, 0);
            
            var totalWeight = alpha * weightRisk + beta * weightTime + gamma * weightPlan;
            
            // Debugging
            Debug.LogWarning($"Node: {node.nodeName} | Risk: {weightRisk} | Planning: {weightPlan} | Time: {weightTime} " +
                             $"| Final Weight: {totalWeight}");
            
            return totalWeight;
        }
        
        private float CalculateEmotionalFactor(Factors factor)
        {
            var factorSum = 0f;
            var nodeCount = 0;
            
            foreach (var child in childNodes)
            {
                float value;
                if (child is not IEmotionable emoChild)
                {
                    Debug.LogError($"{child.nodeName} is not an IEmotionable!");
                    return 0f;
                }
                
                // Get child factor value
                switch (factor)
                {
                    case Factors.Plan:
                        value = emoChild.GetPlanningValue();
                        break;
                    case Factors.Risk:
                        value = emoChild.GetRiskValue();
                        break;
                    case Factors.Time:
                    {
                        (float L, float U) timeRange = emoChild.GetTimeRange();
                        value = (timeRange.L + (timeRange.U - timeRange.L) / 2) * (1 - sigma * opt);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(factor), factor, null);
                }
                
                factorSum += value;
                nodeCount++;
            }
            
            var average = factorSum / nodeCount;
            return average - (-1 * average);
        }
        
        private Node SelectActionWithProbability(List<KeyValuePair<Node, float>> sortedNodes)
        {
            var probabilities = new List<float>();
            var totalProb = 0f;
            
            for (var i = 0; i < sortedNodes.Count; i++)
            {
                var prob = bias * Mathf.Pow(1 - bias, i);
                probabilities.Add(prob);
                totalProb += prob;
            }
            
            var rand = Random.Range(0, totalProb);
            var cumulative = 0f;
            
            for (var i = 0; i < sortedNodes.Count; i++)
            {
                cumulative += probabilities[i];
                if (rand <= cumulative)
                {
                    // Debugging
                    Debug.LogWarning($"Probability: {rand}");
                    Debug.LogWarning($"Cumulative: {cumulative}");
                    Debug.LogError($"Get Node: {sortedNodes[i].Key.nodeName}");
                    
                    return sortedNodes[i].Key;
                }
            }
            return sortedNodes.Last().Key;
        }
    }
}