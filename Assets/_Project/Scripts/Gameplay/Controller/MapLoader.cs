using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

namespace ATBMI.Gameplay.Controller
{
    public class MapLoader : MonoBehaviour
    {
        #region Fields & Properties

        [Serializable]
        private class MapDetail
        {
            public string mapName;
            public GameObject mapObject;
            public Transform mapSpawnPoint;
            public PolygonCollider2D mapConfiner;
        }
        
        [Header("Property")]
        [SerializeField] private string activateMapName;
        [SerializeField] private GameObject player;
        [SerializeField] private MapDetail[] mapDetails;

        private float _loadTime;
        private MapDetail _currentMap;
        
        [Header("Reference")]
        [SerializeField] private FadeController fader;
        [SerializeField] private CinemachineConfiner2D confiner2D;
        
        #endregion

        #region Methods
        
        private void Start()
        {
            _loadTime = fader.FadeDuration * 3f;
            foreach (var map in mapDetails)
            {
                if (map.mapName == activateMapName)
                {
                    _currentMap = map;
                    continue;
                } 
                map.mapObject.SetActive(false);
            }
        }
        
        // Core
        public void LoadMapAsync(string mapName)
        {
            StartCoroutine(LoadMapRoutine(mapName));
        }
        
        private IEnumerator LoadMapRoutine(string mapName)
        {
            var map = Array.Find(mapDetails, x => x.mapName == mapName);
            if (map == null)
            {
                Debug.LogError("map not found!");
                yield break;
            }
            
            fader.FadeOut();
            yield return new WaitForSeconds(_loadTime / 2f);
            _currentMap.mapObject.SetActive(false);
            
            _currentMap = map;
            _currentMap.mapObject.SetActive(true);
            activateMapName = _currentMap.mapName;
            confiner2D.m_BoundingShape2D = _currentMap.mapConfiner;
            player.transform.position = _currentMap.mapSpawnPoint.position;
            
            yield return new WaitForSeconds(_loadTime);
            fader.FadeIn();
        }
        
        #endregion
    }
}
