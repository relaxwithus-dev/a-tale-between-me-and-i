using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

namespace ATBMI.Entities.NPCs
{
    public class EmotionalSelector : Node
    {
        private enum Factors { Plan, Risk, Time }
        private readonly CharacterTraits traits;
        
        // Cached fields
        private readonly float delta = 0.9f;   // Risk impact
        private readonly float lambda = 0.9f;  // Time discounting impact
        private readonly float omega = 0.6f;   // Planning impact
        private readonly float phi = 0.6f;     // Planning weight

        private readonly float bias = 0.75f;  // Adjustable probability
        private readonly float alpha = 1.0f;  // Importance of risk
        private readonly float beta = 1.0f;   // Importance of time
        private readonly float gamma = 1.0f;  // Importance of planning
        
        public EmotionalSelector(string nodeName, CharacterTraits traits, List<Node> childNodes) : base(nodeName, childNodes)
        {
            this.traits = traits;
        }

        public override NodeStatus Evaluate()
        {
            var (dominantEmotion, intensity) = traits.GetDominantEmotion();
            
            // Calculate emotional factor
            float emoPlan = CalculateEmotionalFactor(Factors.Plan);
            float emoRisk = CalculateEmotionalFactor(Factors.Risk);
            float emoTime = CalculateEmotionalFactor(Factors.Time);

            // Calculate impact
            emoPlan = ApplyEmotionImpact(Factors.Plan, emoPlan, dominantEmotion, intensity);
            emoRisk = ApplyEmotionImpact(Factors.Plan, emoRisk, dominantEmotion, intensity);
            emoTime = ApplyEmotionImpact(Factors.Plan, emoTime, dominantEmotion, intensity);
            
            // Compute weighted values for each child node
            var weightedNodes = new Dictionary<Node, float>();
            foreach (var child in childNodes)
            {
                float weight = CalculateWeight(child, emoPlan, emoRisk, emoTime);
                weightedNodes[child] = weight;
            }

            // Sort actions by weight (lower = better)
            var sortedNodes = weightedNodes.OrderBy(kv => kv.Value).ToList();
            var selectedNode = SelectActionWithProbability(sortedNodes);

            return selectedNode.Evaluate();
        }

        private float CalculateWeight(Node node, float emoPlan, float emoRisk, float emoTime)
        {
            if (node is not IEmotionable emoChild)
            {
                Debug.LogError($"{node.nodeName} is not an IEmotionable!");
                return 0f;
            }
            
            float risk = emoChild.GetRiskValue();
            float planning = emoChild.GetPlanningValue();
            (float U, float L) timeRange = emoChild.GetTimeRange();

            float weightPlan = (1 - (1 / (1 + omega * planning))) * Mathf.Max((1 - phi + phi * emoPlan), 0);
            
            float weightRisk = Mathf.Clamp((1 - emoRisk * delta) * risk, 0, 1);
            
            float adjustedTime = timeRange.L + ((timeRange.U - timeRange.L) / 2);
            float weightTime = (1 - (1 / (1 + lambda * adjustedTime))) * Mathf.Max((1 - lambda + lambda * emoTime), 0);
            
            // Final weight calculate
            return alpha * weightRisk + beta * weightTime + gamma * weightPlan;
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

            for (int i = 0; i < sortedNodes.Count; i++)
            {
                cumulative += probabilities[i];
                if (rand <= cumulative)
                    return sortedNodes[i].Key;
            }
            return sortedNodes.Last().Key;
        }
        
        private float CalculateEmotionalFactor(Factors factor)
        {
            var sumPositive = 0f;
            var sumNegative = 0f;
            var countPositive = 0;
            var countNegative = 0;
            
            foreach (var child in childNodes)
            {
                var value = 0f;
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
                        value = timeRange.L + ((timeRange.U - timeRange.L) / 2);
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(factor), factor, null);
                }
                
                // Separate positive and negative values
                if (value > 0)
                {
                    sumPositive += value;
                    countPositive++;
                }
                else if (value < 0)
                {
                    sumNegative += value;
                    countNegative++;
                }
            }

            float positivePart = (countPositive > 0) ? (sumPositive / countPositive) : 0f;
            float negativePart = (countNegative > 0) ? (sumNegative / countNegative) : 0f;

            return positivePart - negativePart;
        }
        
        private float ApplyEmotionImpact(Factors factor, float emotionalFactor, EmotionType emotion, float intensity)
        {
            var emotionImpact = new Dictionary<EmotionType, Dictionary<Factors, float>>
            {
                { EmotionType.Joy, new Dictionary<Factors, float> { { Factors.Plan, 1 }, { Factors.Risk, 1 }, { Factors.Time, 1 } } },
                { EmotionType.Trust, new Dictionary<Factors, float> { { Factors.Plan, -1 }, { Factors.Risk, 1 }, { Factors.Time, 1 } } },
                { EmotionType.Fear, new Dictionary<Factors, float> { { Factors.Plan, -1 }, { Factors.Risk, -1 }, { Factors.Time, -1 } } },
                { EmotionType.Surprise, new Dictionary<Factors, float> { { Factors.Plan, -1 }, { Factors.Risk, 1 }, { Factors.Time, -1 } } },
                { EmotionType.Sadness, new Dictionary<Factors, float> { { Factors.Plan, -1 }, { Factors.Risk, -1 }, { Factors.Time, 1 } } },
                { EmotionType.Anger, new Dictionary<Factors, float> { { Factors.Plan, -1 }, { Factors.Risk, 1 }, { Factors.Time, -1 } } },
            };
            
            var impact = emotionImpact.ContainsKey(emotion) && emotionImpact[emotion].ContainsKey(factor) 
                ? emotionImpact[emotion][factor] : 0;

            return emotionalFactor * (1 + (impact * intensity));
        }
        
    }
}