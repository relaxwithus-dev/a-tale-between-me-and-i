using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class LunaBT : EmoTrees
    {
        [Header("Attribute")] 
        [SerializeField] private float runAwayDuration = 8f;
        [SerializeField] private Transform targetPoint;
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            Selector tree = new Selector("Luna BT", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    // Public
                    new Sequence("Public Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[0].Radius, layerMask),
                        new EmotionalSelector("Fear and Disgust", characterTraits, new List<Node>
                        {
                            new TaskRunAway(characterAI, characterAnim, characterAI.Data, runAwayDuration),
                            new TaskAnimate(characterAnim, StateTag.AFRAID_STATE),
                            new TaskIdle(characterAI)
                        })
                    })
                }),
                new Sequence("Move", new List<Node>
                {
                    new CheckReachTarget(characterAI, targetPoint),
                    new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true, targetPoint),
                    new TaskIdle(characterAI)
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}