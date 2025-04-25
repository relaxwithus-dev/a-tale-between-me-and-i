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
            var angerText = GetTextAssets(Emotion.Anger);
            
            Selector tree = new Selector("Toko Kelontong BT", new List<Node>
            {
                new CheckInteracted(interact),
                new ZoneSelector("Proxemics", new List<Node>
                {
                    // Intimate
                    new Sequence("Intimate Zone", new List<Node>
                    {
                        new CheckTargetInProxemics(centerPoint, zoneDetails[0].Radius, layerMask),
                        new EmotionalSelector("Anger", characterTraits, new List<Node>
                        {
                            new TaskTalk(characterAI, angerText),
                            new TaskIdle(characterAI)
                        })
                    }),
                    // Personal
                    new Sequence("Personal Zone", new List<Node>
                    {
                        new CheckTargetInProxemics(centerPoint, zoneDetails[1].Radius, layerMask),
                        new EmotionalSelector("Disgust", characterTraits, new List<Node>
                        {
                            new TaskAnimate(characterAnim, "Disgust"),
                            new TaskTalk(characterAI, defaultTexts),
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
