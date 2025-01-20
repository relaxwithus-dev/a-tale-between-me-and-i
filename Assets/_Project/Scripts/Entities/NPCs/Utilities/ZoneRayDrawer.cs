using UnityEngine;

namespace ATBMI.Entities.NPCs
{ 
    [RequireComponent(typeof(SatpamBT))] 
    public class ZoneRayDrawer : MonoBehaviour
    {
        // Reference
        private SatpamBT _characterBT;
        
        private void Awake()
        {
            _characterBT = GetComponent<SatpamBT>();
        }
        
        private void OnDrawGizmos()
        {
            if (_characterBT == null) return;
            foreach (var space in _characterBT.ZoneDetails)
            {
                var targetColor = GetColor(space.Type);
                Gizmos.color = targetColor;
                Gizmos.DrawWireSphere(transform.position, space.Radius);
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
    }
}