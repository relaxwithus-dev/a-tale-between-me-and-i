using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ATBMI.Scene
{
    [CreateAssetMenu(fileName = "NewSceneAsset", menuName = "Data/Scene Asset", order = 0)]
    public class SceneAsset : ScriptableObject
    {
        #region Struct
        
        [Serializable]
        public class SceneNeighbour
        {
            public SceneAsset sceneAsset;
            public Transform entryPointFrom;
        }
        
        #endregion
        
        [Header("Assets")] 
        [SerializeField] private string sceneId;
        [SerializeField] private Collider2D confiner;
        [SerializeField] private SceneReference reference;
        [SerializeField] private List<SceneNeighbour> neighbours;
        
        // Getter
        public string Id => sceneId;
        public Collider2D Confiner => confiner;
        public SceneReference Reference => reference;
        public SceneNeighbour GetNeighbourById(string id)
        {
            var neighbour = neighbours.Find(x => x.sceneAsset.sceneId == id);
            if (neighbour == null)
            {
                Debug.LogWarning("neighbour not found!");
                return null;
            }
            
            return neighbour;
        }
    }
}