using ATBMI.Interaction;
using UnityEngine;

namespace ATBMI.Entities.NPCs
{
    [RequireComponent(typeof(CharacterTraits))]
    public class EmoTrees : Trees
    {
        #region Fields & Properties

        [Header("Zone")] 
        [SerializeField] protected Transform centerPoint;
        [SerializeField] protected ZoneDetail[] zoneDetails;
        [SerializeField] protected LayerMask layerMask;
        
        public ZoneDetail[] ZoneDetails => zoneDetails;
        
        [Header("Reference")]
        [SerializeField] protected CharacterInteract characterInteract;
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
        
        // Drawer
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