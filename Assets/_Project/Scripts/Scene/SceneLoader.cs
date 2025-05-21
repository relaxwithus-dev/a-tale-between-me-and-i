using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using ATBMI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Scene
{
    [DisallowMultipleComponent]
    public class SceneLoader : MonoBehaviour
    {
        #region Struct

        [Serializable]
        private struct EntryInfo
        {
            public LocationData locationData;
            public Transform pointFromScene;
        }

        #endregion

        #region Fields & Properties
        
        [Header("Attribute")]
        [SerializeField] private Transform defaultPoint;
        [SerializeField] private EntryInfo[] entryPoints;
        [SerializeField] private PolygonCollider2D confiner;
        
        // Reference
        protected PlayerController player;
        private FadeController _fader;
        private CinemachineVirtualCamera[] _virtualCameras;
        
        #endregion

        #region Methods

        // Unity Callbacks
        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
            
            _fader = SceneNavigation.Instance.Fader;
            _virtualCameras = CameraSwitcher.Instance.VirtualCameras;
        }
        
        private void OnEnable()
        {
            SetupSceneAttribute();
            StartCoroutine(AnimateSceneFade());
        }
        
        private void SetupSceneAttribute()
        {
            var latestScene = SceneNavigation.Instance.LatestScene;
            var entryPoint = latestScene != null && latestScene.Type == SceneAsset.SceneType.Gameplay
                ? Array.Find(entryPoints, e => e.locationData.location == latestScene.Id).pointFromScene 
                : defaultPoint;

            if (entryPoint == null)
            {
                Debug.LogWarning("entry point not found!");
                return;
            }
            
            player.transform.position = entryPoint.position;
            foreach (var vCam in _virtualCameras)
            {
                var vConfiner = vCam.GetComponent<CinemachineConfiner2D>();
                if (vConfiner == null)
                {
                    Debug.LogWarning("camera confiner not found!");
                    break;
                }
                vConfiner.m_BoundingShape2D = confiner;
                vConfiner.InvalidateCache();
            }
        }
        
        protected virtual IEnumerator AnimateSceneFade()
        {
            yield return new WaitForSeconds(_fader.FadeDuration);
            _fader.FadeIn(() =>
            {
                player.StartMovement();
                DialogueEvents.RegisterDialogueSignPointEvent();
            });
        }
        
        #endregion
    }
}