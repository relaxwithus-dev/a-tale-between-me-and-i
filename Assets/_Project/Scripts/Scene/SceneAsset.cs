using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ATBMI.Scene
{
    [CreateAssetMenu(fileName = "NewSceneAsset", menuName = "Data/Scene/Scene Asset", order = 0)]
    public class SceneAsset : ScriptableObject
    {
        public enum SceneType { Global, Gameplay }
        
        [Header("Assets")]
        [SerializeField] private SceneType sceneType;
        [SerializeField] [ShowIf("sceneType", SceneType.Gameplay)]
        private LocationData location;
        [SerializeField] private SceneReference reference;
        [SerializeField] private List<SceneAsset> neighbours;
        
        // Getter
        public SceneType Type => sceneType;
        public Location Id => location.location;
        public Region Region => location.region;
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