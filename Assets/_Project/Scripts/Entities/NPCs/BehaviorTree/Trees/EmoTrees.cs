using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [RequireComponent(typeof(CharacterTraits))]
    public class EmoTrees : Trees
    {
        [Header("Zone")] 
        [SerializeField] protected Transform centerPoint;
        [SerializeField] protected ZoneDetail[] zoneDetails;
        [SerializeField] protected LayerMask layerMask;
        
        public ZoneDetail[] ZoneDetails => zoneDetails;
        
        // Reference
        protected CharacterTraits characterTraits;
        
        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            characterTraits = GetComponent<CharacterTraits>();
        }
        
        protected override Node SetupTree()
        {
            throw new System.NotImplementedException();
        }
    }
}