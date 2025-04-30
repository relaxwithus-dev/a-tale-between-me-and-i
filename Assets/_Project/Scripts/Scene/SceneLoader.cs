using System;
using ATBMI.Entities.Player;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace ATBMI.Scene
{
    public class SceneLoader : MonoBehaviour
    {
        #region Struct

        [Serializable]
        private struct EntryInfo
        {
            [FormerlySerializedAs("locationTarget")] public LocationData locationData;
            public Transform pointFromScene;
        }

        #endregion

        #region Fields & Properties

        [Header("Attribute")]
        [SerializeField] private bool isFirstSpawn;
        [SerializeField] private Transform defaultPoint;
        [SerializeField] private EntryInfo[] entryPoints;
        [SerializeField] private PolygonCollider2D confiner;
        
        // Reference
        private PlayerController _player;
        private CinemachineConfiner2D _cameraConfiner;
        
        #endregion

        #region Methods

        // Unity Callbacks
        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
            _cameraConfiner = FindObjectOfType<CinemachineConfiner2D>();
        }
        
        private void OnEnable()
        {
            var latestScene = SceneNavigation.Instance.LatestScene;
            var entryPoint = latestScene != null 
                ? Array.Find(entryPoints, e => e.locationData.location == latestScene.Id).pointFromScene 
                : defaultPoint;

            if (entryPoint == null)
            {
                Debug.LogWarning("entry point not found!");
                return;
            }
            
            _player.transform.position = entryPoint.position;
            _cameraConfiner.m_BoundingShape2D = confiner;
            _cameraConfiner.InvalidateCache();
        }
        
        #endregion
    }
}