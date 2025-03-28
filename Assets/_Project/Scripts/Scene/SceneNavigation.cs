using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Scene
{
    public class SceneNavigation : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Properties")]
        [SerializeField] private bool isInitialized;
        [SerializeField] private SceneAsset initializeScene;
        [SerializeField] private FadeController fader;
        
        private AsyncOperation _asyncOperation;

        public SceneAsset CurrentScene { get; private set; }
        public SceneAsset LatestScene { get; private set; }
        public static SceneNavigation Instance;

        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            // Initialize scene
            if (isInitialized) return;
            StartCoroutine(InitSceneAsync());
        }
        
        // Initialize
        private IEnumerator InitSceneAsync()
        {
            fader.DoFade(1f, 0f);
            
            StartCoroutine(LoadSceneAsyncProcess(initializeScene.Reference)); 
            _asyncOperation.allowSceneActivation = true;
            CurrentScene = initializeScene;
            
            yield return new WaitForSeconds(fader.FadeDuration);
            fader.FadeIn();
        }
        
        // Core
        public void SwitchScene(SceneAsset sceneAsset)
        {
            fader.FadeOut();
            if (CurrentScene)
                LatestScene = CurrentScene;
            
            // StartCoroutine(LoadSceneAsyncProcess(sceneAsset.Reference)); 
            SceneManager.LoadSceneAsync(sceneAsset.Reference, LoadSceneMode.Additive);
            _asyncOperation.allowSceneActivation = true;
            CurrentScene = sceneAsset;
            
            SceneManager.UnloadSceneAsync(LatestScene.Reference);
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

        #endregion

    }
}