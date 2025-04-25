using System;
using UnityEngine;
using ATBMI.Interaction;
using UnityEngine.Serialization;

namespace ATBMI.Entities.NPCs
{
    [RequireComponent(typeof(CharacterTraits))]
    public class EmoTrees : Trees
    {
        [Serializable]
        private struct DialogueAsset
        {
            public Emotion emotion;
            public TextAsset[] textAssets;
        }
        
        #region Fields & Properties
        
        [Header("Dialogue")]
        [SerializeField] protected TextAsset[] defaultTexts;
        [SerializeField] private DialogueAsset[] emotionTexts;
        [SerializeField] protected Animator emoteAnimator;
        
        [Header("Zone")] 
        [SerializeField] protected Transform centerPoint;
        [SerializeField] protected ZoneDetail[] zoneDetails;
        [SerializeField] protected LayerMask layerMask;
        
        public ZoneDetail[] ZoneDetails => zoneDetails;
        
        [Header("Reference")]
        [SerializeField] protected CharacterInteract interact;
        protected CharacterTraits characterTraits;

        #endregion

        #region Methods

        // Initialize
        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            characterTraits = GetComponent<CharacterTraits>();
        }

        protected override Node SetupTree()
        {
            throw new System.NotImplementedException();
        }
        
        // Helper
        protected TextAsset[] GetTextAssets(Emotion emotion)
        {
            var asset = Array.Find(emotionTexts, x => x.emotion == emotion);
            return asset.textAssets;
        }
        
        private void OnDrawGizmos()
        {
            if (zoneDetails == null || zoneDetails.Length < 1) return;
            foreach (var space in zoneDetails)
            {
                Gizmos.color = GetColor(space.Type);
                Gizmos.DrawWireSphere(centerPoint.position, space.Radius);
            }
        }
        
        private Color GetColor(ZoneType space)
        {
            return space switch
            {
                ZoneType.Intimate => Color.magenta,
                ZoneType.Personal => Color.green,
                ZoneType.Public => Color.blue,
                _ => Color.black
            };
        }

        #endregion
        
    }
}