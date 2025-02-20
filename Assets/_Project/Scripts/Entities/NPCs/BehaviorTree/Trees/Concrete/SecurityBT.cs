using System.Collections.Generic;
using ATBMI.Interaction;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class SecurityBT : EmoTrees
    {
        // Reference
        [SerializeField] private CharacterInteract characterInteract;

        protected override Node SetupTree()
        {
           EmotionalSelector intimateEmoSelector = new EmotionalSelector("Intimate Emo", characterTraits, 
                new List<Node>
                {
                    new Selector("Anticipation", new List<Node>
                    {
                        new Sequence("Behavior A", new List<Node>
                        {
                            // new CheckInteractCount(this, 1),
                            new TaskDialogue(characterAI, dialogueText: "Aku Antisipasi!")
                        }),
                        new TaskIdle(characterAI)
                    })
                });
            
            EmotionalSelector personalEmoSelector = new EmotionalSelector("Personal Emo", characterTraits, 
                new List<Node>
                {
                    new Selector("Anger", new List<Node>
                    {
                        new Sequence("Behavior A", new List<Node>
                        {
                            // new CheckInteractCount(this, 1),
                            new TaskDialogue(characterAI, dialogueText: "Aku Marah!")
                        }),
                        new Sequence("Behavior B", new List<Node>
                        {
                            // new CheckInteractCount(this, 2),
                            new Sequence("Push", new List<Node>
                            {
                                new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true, isAway: false),
                                new TaskPush(),
                                new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true, isAway: true)
                            }),
                            new TaskDialogue(characterAI, dialogueText: "Aku Marah Lo!")
                        }),
                        new TaskIdle(characterAI)
                    })
                });
            
            Selector tree = new Selector(rootName, new List<Node>
            {
                new ZoneSelector("Zone", new List<Node>
                {
                    new Sequence(zoneDetails[0].Type.ToString(), new List<Node>
                    {
                        // new CheckTargetInZone(centerPoint, zoneDetails[0].Radius, layerMask),
                         new EmotionalSelector("Intimate Emo", characterTraits, new List<Node> 
                         {
                            new SelectorWeight("Anticipation", new List<Node>
                            {
                                new SequenceWeight("Behavior A", new List<Node>
                                {
                                    // new CheckInteractCount(this, 1),
                                    new TaskDialogue(characterAI, dialogueText: "Aku Antisipasi!")
                                }),
                                new TaskIdle(characterAI)
                            }) 
                         })
                    }),
                    new Sequence(zoneDetails[1].Type.ToString(), new List<Node>
                    {
                        // new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Personal Emo", characterTraits, new List<Node>
                        {
                            new SelectorWeight("Anger", new List<Node>
                            {
                                new SequenceWeight("Behavior A", new List<Node>
                                {
                                    // new CheckInteractCount(this, 1),
                                    new TaskDialogue(characterAI, dialogueText: "Aku Marah!")
                                }),
                                new SequenceWeight("Behavior B", new List<Node>
                                {
                                    // new CheckInteractCount(this, 2),
                                    new SequenceWeight("Push", new List<Node>
                                    {
                                        new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true, isAway: false),
                                        new TaskPush(),
                                        new TaskMoveToTarget(characterAI, characterAI.Data, isWalk: true, isAway: true)
                                    }),
                                    new TaskDialogue(characterAI, dialogueText: "Aku Marah Lo!")
                                }),
                                new TaskIdle(characterAI)
                            })
                        })
                    })
                }),
                new TaskIdle(characterAI)
            });

            return tree;
        }
    }
}
