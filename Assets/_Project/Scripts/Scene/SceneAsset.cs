using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ATBMI.Scene
{
    [CreateAssetMenu(fileName = "NewSceneAsset", menuName = "Data/Scene/Scene Asset", order = 0)]
    public class SceneAsset : ScriptableObject
    {
        [FormerlySerializedAs("locationData")]
        [Header("Assets")] 
        [SerializeField] private LocationTarget locationTarget;
        [SerializeField] private SceneReference reference;
        [SerializeField] private List<SceneAsset> neighbours;
        
        // Getter
        public Location Id => locationTarget.location;
        public SceneReference Reference => reference;
        public SceneAsset GetNeighbourById(Location id)
        {
            var neighbour = neighbours.Find(x => x.Id == id);
            if (neighbour == null)
            {
                Debug.LogWarning("neighbour not found!");
                return null;
            }
            
            return neighbour;
        }
    }
}