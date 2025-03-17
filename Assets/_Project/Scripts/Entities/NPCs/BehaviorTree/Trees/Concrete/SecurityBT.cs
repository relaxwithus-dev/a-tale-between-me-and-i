using System.Collections.Generic;
using UnityEngine;
using ATBMI.Interaction;

namespace ATBMI.Entities.NPCs
{
    public class SecurityBT : EmoTrees
    {
        [Header("Property")] 
        [SerializeField] private float pullForce;
        [SerializeField] private float pullDelay;
        [SerializeField] private float passOffRange;
        
        [Space]
        [SerializeField] private CharacterAnimation characterAnimation;
        
        protected override Node SetupTree()
        {
            Selector tree = new Selector("Security", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    // Intimate
                    new Sequence("Intimate Zone", new List<Node>
                    {
                        new CheckTargetInProxemics(centerPoint, zoneDetails[0].Radius, layerMask),
                        new EmotionalSelector("Anger", characterTraits, new List<Node>
                        {
                            new SequenceWeight("Pull",new List<Node>
                            {
                                new CheckInAction(isAction: true),
                                new CheckPassed(characterAI, zoneDetails[1].Radius),
                                new TaskPull(characterAI, pullForce, pullDelay),
                                new TaskTalk(characterAI, CharacterState.Anger, "hei, yang sopan kamu!"),
                                new CheckInAction(isAction: false)
                            }),
                            new TaskTalk(characterAI, "mau kemana kamu?"),
                            new TaskIdle(characterAI)
                        })
                    }),
                    // Personal
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInProxemics(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Anticipation", characterTraits, new List<Node>
                        {
                            new TaskObserve(characterAI, characterAnimation, zoneDetails[1].Radius),
                            new TaskAnimate(characterAnimation, "Anticipation"),
                            new TaskIdle(characterAI)
                        })
                    })
                }),
                new TaskIdle(characterAI)
            });
            
            return tree;
        }
    }
}
