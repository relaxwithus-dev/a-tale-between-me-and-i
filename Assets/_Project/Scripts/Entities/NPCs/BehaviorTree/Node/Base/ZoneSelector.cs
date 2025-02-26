using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;

namespace ATBMI.Entities.NPCs
{
    public class ZoneSelector : Node
    {
        private readonly Dictionary<Node, float> zoneWeights = new ();
        private readonly float decayRate = 0.1f;
        private Node _selectedZone;

        private static Random _rng = new();
        
        public ZoneSelector(string nodeName, List<Node> childNodes) : base(nodeName, childNodes)
        {
            foreach (var child in childNodes)
            {
                zoneWeights[child] = 1f;
            }
        }
        
        public override NodeStatus Evaluate()
        {
            if (_selectedZone == null)
            {
                _selectedZone = TrySelectZone();
                Debug.Log($"Execute Zone: {_selectedZone.nodeName}");
            }
            
            switch (_selectedZone.Evaluate())
            {
                case NodeStatus.Running:
                    return NodeStatus.Running;
                case NodeStatus.Success:
                    _selectedZone = null;
                    return NodeStatus.Success;
                case NodeStatus.Failure:
                default:
                    Reset();
                    return NodeStatus.Failure;
            }
        }
        
        private Node TrySelectZone()
        {
            // Decay old weights
            foreach (var node in zoneWeights.Keys.ToList())
                zoneWeights[node] *= (1 - decayRate);
            
            var totalWeight = zoneWeights.Values.Sum();
            var randomValue = _rng.NextDouble() * totalWeight;
            var cumulativeWeight = 0f;
            
            // Random weight selection
            foreach (var node in childNodes)
            {
                cumulativeWeight += zoneWeights.GetValueOrDefault(node, 0f);
                if (randomValue <= cumulativeWeight)
                {
                    zoneWeights[node] += 1f;
                    return node;
                }
            }

            return childNodes.Last();
        }
    }
}