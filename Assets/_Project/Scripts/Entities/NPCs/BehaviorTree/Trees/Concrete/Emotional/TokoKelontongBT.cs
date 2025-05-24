using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class TokoKelontongBT : EmoTrees
    {
        [Header("Attribute")]
        [SerializeField] private CharacterAnimation characterAnim;
        
        protected override Node SetupTree()
        {
            var data = characterAI.Data;
            var angerTexts = data.GetEmotionDialogues(Emotion.Anger);
            var disgustTexts = data.GetEmotionDialogues(Emotion.Disgust);
            
            Selector tree = new Selector("Toko Kelontong BT", new List<Node>
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
                            new TaskTalk(characterAI, angerTexts),
                            new TaskIdle(characterAI)
                        })
                    }),
                    // Personal
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInZone(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Disgust", characterTraits, new List<Node>
                        {
                            new TaskAnimate(characterAnim, StateTag.DISGUST_STATE),
                            new TaskTalk(characterAI, disgustTexts),
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
