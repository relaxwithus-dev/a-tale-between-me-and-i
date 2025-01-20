using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    public class SatpamBT : Trees
    {
        #region Fields & Properties

        [Header("Zone")]
        [SerializeField] private ZoneDetail[] zoneDetails;
        [SerializeField] private LayerMask layerMask;
        
        public ZoneDetail[] ZoneDetails => zoneDetails;
        
        // Reference
        private CharacterAI _characterAI;

        #endregion

        protected override void InitOnAwake()
        {
            base.InitOnAwake();
            _characterAI = GetComponent<CharacterAI>();
        }
        
        protected override Node SetupTree()
        {
            throw new System.NotImplementedException();
        }
    }
}