using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using ATBMI.Singleton;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Scene
{
    public class SceneNavigation : MonoDDOL<SceneNavigation>
    {
        // Fields & Properties
        [Header("Properties")]
        [SerializeField] private SceneAsset initializeScene;
        [SerializeField] private bool isInitialized;
        
        [Space]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private CinemachineConfiner2D cameraConfiner;
        [SerializeField] private FadeController fader;
        
        private SceneAsset _currentScene;
        private SceneAsset _latestScene;
        private AsyncOperation _asyncOperation;
        
        // Methods
        private void Start()
        {
            // Initialize Scene
            if (isInitialized) return;
            _currentScene = initializeScene;
            StartCoroutine(LoadSceneAsyncProcess(_currentScene.Reference));
        }
        
        public void SwitchScene(SceneAsset sceneAsset)
        {
            StartCoroutine(SwitchRoutine(sceneAsset));
        }
        
        private IEnumerator SwitchRoutine(SceneAsset sceneAsset)
        {
            fader.FadeOut();
            if (_currentScene)
                _latestScene = _currentScene;
            
            // Setup properties
            var fromNeighbour = sceneAsset.GetNeighbourById(_latestScene.Id);
            playerTransform.position = fromNeighbour.entryPointFrom.position;
            cameraConfiner.m_BoundingShape2D = sceneAsset.Confiner;
            cameraConfiner.InvalidateCache();
            
            // Load scene
            yield return LoadSceneAsyncProcess(sceneAsset.Reference);
            _asyncOperation.allowSceneActivation = true;
            _currentScene = sceneAsset;
            
            UnloadSceneAsyncProcess(_latestScene.Reference);
            fader.FadeIn();
        }
        
        private IEnumerator LoadSceneAsyncProcess(SceneReference scene)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            if (_asyncOperation != null)
            {
                _asyncOperation.allowSceneActivation = false;
                while (!_asyncOperation.isDone)
                    yield return null;
            }
        }
        
        private void UnloadSceneAsyncProcess(SceneReference scene)
        {
            _asyncOperation = SceneManager.UnloadSceneAsync(scene);
        }
    }
}