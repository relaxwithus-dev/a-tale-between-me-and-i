using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class LunaBT : EmoTrees
    {
        [Header("Attribute")] 
        [SerializeField] private float runAwayDuration = 8f;
        [SerializeField] private Transform targetPoint;
        
        [Space]
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            var defaultTexts = characterAI.Data.GetDefaultDialogue();
            
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
                            new TaskRunAway(characterAI, characterAI.Data, runAwayDuration),
                            new TaskTalk(characterAI, defaultTexts),
                            new TaskAnimate(characterAnim, "Disgust"),
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