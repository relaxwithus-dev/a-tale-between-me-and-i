using System.Linq;
using UnityEngine;
using UnityEditor;

namespace ATBMI.Scene
{
    [CreateAssetMenu(fileName = "NewLocationData", menuName = "Data/Scene/Location Data", order = 1)]
    public class LocationTarget : ScriptableObject
    {
        [Header("Attribute")] 
        public Region region;
        public Location location;
        
        public string GetRegionAcronym(Region targetRegion)
        {
            return targetRegion switch
            {
                Region.Surabaya => "SBY",
                Region.Bali => "BLI",
                _ => "UNK"
            };
        }
    }
    
    [CustomEditor(typeof(LocationTarget))]
    public class LocationDataEditor : Editor 
    {
        public override void OnInspectorGUI() 
        {
            var selector = (LocationTarget)target;
            var regionName = selector.region.ToString();
            selector.region = (Region)EditorGUILayout.EnumPopup("Region", selector.region);

            if (!string.IsNullOrEmpty(regionName))
            {
                var allLocations = System.Enum.GetValues(typeof(Location)).Cast<Location>();
                var filtered = allLocations
                    .Where(loc => loc.ToString().StartsWith(selector.GetRegionAcronym(selector.region) + "_"))
                    .ToArray();
                
                if (filtered.Length > 0) 
                {
                    var currentIndex = System.Array.IndexOf(filtered, selector.location);
                    if (currentIndex == -1) currentIndex = 0;

                    selector.location = filtered[
                        EditorGUILayout.Popup("Location", currentIndex,
                            filtered.Select(l => l.ToString()).ToArray())
                    ];
                } else 
                {
                    EditorGUILayout.HelpBox("Tidak ada lokasi untuk kota ini.", MessageType.Info);
                }
            } else 
            {
                EditorGUILayout.HelpBox("Masukkan nama kota terlebih dahulu.", MessageType.Warning);
            }
            
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}