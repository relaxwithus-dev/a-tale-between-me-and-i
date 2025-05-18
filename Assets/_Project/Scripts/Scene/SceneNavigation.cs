using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using ATBMI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;
using ATBMI.Managers;

namespace ATBMI.Scene
{
    public class SceneNavigation : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Attribute")]
        [SerializeField] private bool debugMode;
        [SerializeField] private SceneAsset menuScene;
        [SerializeField] [ShowIf("debugMode")]
        private SceneAsset debugScene;
        
        private AsyncOperation _asyncOperation;
        
        public SceneAsset CurrentScene { get; private set; }
        public SceneAsset LatestScene { get; private set; }
        
        public static SceneNavigation Instance;
        
        [Header("Reference")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PlayerController player;
        [SerializeField] private FadeController fader;
        
        public FadeController Fader => fader;
                
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        
        private void Start()
        {
            StartCoroutine(InitSceneAsync(debugMode ? debugScene : menuScene));
        }
        
        // Initialize
        private IEnumerator InitSceneAsync(SceneAsset sceneAsset)
        {
            fader.gameObject.SetActive(true);
            fader.DoFade(1f, 0f);
            
            StartCoroutine(LoadSceneAsync(sceneAsset.Reference));
            _asyncOperation.allowSceneActivation = true;
            CurrentScene = sceneAsset;
            
            yield return new WaitForSeconds(fader.FadeDuration);
            fader.FadeIn();
            if (debugMode)
            {
                gameManager.StartGame();
                DialogueEvents.RegisterDialogueSignPointEvent();
            }
            else
            {
                gameManager.EndGame();
            }
        }
        
        // Core
        public void SwitchScene(SceneAsset sceneAsset)
        {
            StartCoroutine(SwitchRoutine(sceneAsset));
        }
        
        public void SwitchSceneSection(bool isToMenu, SceneAsset sceneAsset = null)
        {
            // Validate
            if (!isToMenu && sceneAsset == null)
            {
                Debug.LogWarning("scene asset is null!");
                return;
            }
            
            var sceneTarget = isToMenu ? menuScene : sceneAsset;
            Action modifyGameplayAction = isToMenu 
                ? () => gameManager.EndGame() 
                : () => gameManager.StartGame();
            
            StartCoroutine(SwitchRoutine(sceneTarget, modifyGameplayAction));
        }
        
        private IEnumerator SwitchRoutine(SceneAsset sceneAsset, Action onSwitchBegin = null)
        {
            fader.FadeOut();
            player.StopMovement();
            yield return new WaitForSeconds(fader.FadeDuration);
            
            onSwitchBegin?.Invoke();
            DialogueEvents.UnregisterDialogueSignPointEvent();
            
            // Setup latest scene
            if (CurrentScene != null)
                LatestScene = CurrentScene;
            
            StartCoroutine(LoadSceneAsync(sceneAsset.Reference));
            _asyncOperation.allowSceneActivation = true;
            CurrentScene = sceneAsset;
            
            UnloadSceneAsync(LatestScene.Reference);
        }
        
        private IEnumerator LoadSceneAsync(SceneReference scene)
        {
            _asyncOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            if (_asyncOperation != null)
            {
                _asyncOperation.allowSceneActivation = false;
                while (!_asyncOperation.isDone)
                    yield return null;
            }
        }
        
        private void UnloadSceneAsync(SceneReference scene)
        {
            _asyncOperation = SceneManager.UnloadSceneAsync(scene);
        }

        // private void ModifyGameplay(bool isEnable)
        // {
        //     foreach (var obj in gameplayObjects)
        //     {
        //         obj.SetActive(isEnable);
        //     }
        // }

        #endregion
    }
}