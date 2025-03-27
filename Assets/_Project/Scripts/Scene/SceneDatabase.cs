using System.Collections.Generic;
using UnityEngine;
using Project.Tools.DictionaryHelp;
using ATBMI.Singleton;

namespace ATBMI.Scene
{
    public class SceneDatabase : MonoDDOL<SceneDatabase>
    {
        // Fields
        [SerializeField] private SerializableDictionary<string, List<SceneAsset>> sectionScenes;
        
        // Methods
        public SceneAsset GetSceneAsset(string region, string sceneId)
        {
            if (!sectionScenes.ContainsKey(region))
                return null;
            
            foreach (var scene in sectionScenes[region])
            {
                if (scene == null)
                {
                    Debug.LogWarning("scene asset is null!");
                    return null;
                }
                
                if (scene.Id == sceneId)
                    return scene;
            }
            return null;
        }
    }
}