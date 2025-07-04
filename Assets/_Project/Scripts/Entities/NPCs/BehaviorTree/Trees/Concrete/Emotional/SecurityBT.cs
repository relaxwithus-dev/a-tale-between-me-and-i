using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class SecurityBT : EmoTrees
    {
        [Header("Attribute")] 
        [SerializeField] private float pullForce;
        [SerializeField] private float pullDelay;
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            var data = characterAI.Data;
            var defaultTexts = data.GetDefaultDialogues();
            var angerTexts = data.GetEmotionDialogues(Emotion.Anger);
            
            Selector tree = new Selector("Security BT", new List<Node>
            {
                new CheckInteracted(characterInteract),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    // Intimate
                    new Sequence("Intimate Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[0].Radius, layerMask),
                        new EmotionalSelector("Anger", characterTraits, new List<Node>
                        {
                            new SequenceWeight("Pull",new List<Node>
                            {
                                new CheckPassed(characterAI, zoneDetails[1].Radius),
                                new TaskPull(characterAI,characterAnim, pullForce, pullDelay),
                                new TaskTalk(characterAI, characterAnim, angerTexts, isState: true)
                            }),
                            new TaskTalk(characterAI, characterAnim, defaultTexts),
                            new TaskIdle(characterAI)
                        })
                    }),
                    // Personal
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Anticipation", characterTraits, new List<Node>
                        {
                            new TaskObserve(characterAI, characterAnim, zoneDetails[1].Radius),
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
