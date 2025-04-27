using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ATBMI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Scene
{
    public class SceneNavigation : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Properties")]
        [SerializeField] private bool isInitialized;
        [SerializeField] private SceneAsset initializeScene;

        [Space]
        [SerializeField] private PlayerController player;
        [SerializeField] private FadeController fader;

        private AsyncOperation _asyncOperation;
        
        public SceneAsset CurrentScene { get; private set; }
        public SceneAsset LatestScene { get; private set; }
        public string CurrentSceneName => CurrentScene.name;
        
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
            fader.gameObject.SetActive(true);
            fader.DoFade(1f, 0f);

            StartCoroutine(LoadSceneAsyncProcess(initializeScene.Reference));
            _asyncOperation.allowSceneActivation = true;
            CurrentScene = initializeScene;

            yield return new WaitForSeconds(fader.FadeDuration);
            fader.FadeIn();

            DialogueEvents.RegisterDialogueSignPointEvent(); // Register NPCs sign point
        }

        // Core
        public void SwitchScene(SceneAsset sceneAsset)
        {
            StartCoroutine(SwitchRoutine(sceneAsset));
        }

        private IEnumerator SwitchRoutine(SceneAsset sceneAsset)
        {
            fader.FadeOut();
            player.StopMovement();
            yield return new WaitForSeconds(fader.FadeDuration);

            DialogueEvents.UnregisterDialogueSignPointEvent(); // Unregister old NPCs sign point before switching scenes

            if (CurrentScene)
            {
                LatestScene = CurrentScene;
            }

            StartCoroutine(LoadSceneAsyncProcess(sceneAsset.Reference));
            _asyncOperation.allowSceneActivation = true;
            CurrentScene = sceneAsset;

            UnloadSceneAsyncProcess(LatestScene.Reference);
            yield return new WaitForSeconds(fader.FadeDuration * 2.5f);
            fader.FadeIn(() =>
            {
                player.StartMovement();
            });
            
            DialogueEvents.RegisterDialogueSignPointEvent(); // Register NPCs sign point
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