using System.Collections.Generic;
using UnityEngine;

namespace ATBMI.Scene
{
    [CreateAssetMenu(fileName = "NewSceneAsset", menuName = "Data/Scene Asset", order = 0)]
    public class SceneAsset : ScriptableObject
    {
        [Header("Assets")] 
        [SerializeField] private string sceneId;
        [SerializeField] private SceneReference reference;
        [SerializeField] private List<SceneAsset> neighbours;
        
        // Getter
        public string Id => sceneId;
        public SceneReference Reference => reference;
        public SceneAsset GetNeighbourById(string id)
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