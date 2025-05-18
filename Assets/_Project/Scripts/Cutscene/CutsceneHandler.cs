using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Core;
using ATBMI.Entities;

namespace ATBMI.Cutscene
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CutsceneHandler : MonoBehaviour
    {
        #region Method
        
        [Header("Attribute")]
        [SerializeField] private string cutsceneID;
        [SerializeField] private float transitionTime;
        [SerializeField] private List<Cutscene> cutsceneSteps;
        
        private int _currentIndex;
        private bool _isPlaying;
        private bool _isTransitioning;
        private bool _hasExecutingStep;
        
        private Cutscene _currentCutscene;
        
        // Reference
        private IController _iController;
        private BoxCollider2D _boxCollider2D;
        public CutsceneDirector CutsceneDirector { get; set; }
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            InitAttribute();
            SetupCollider(isEnable: true);
        }
        
        private void Update()
        {
            if (!_isPlaying || _isTransitioning) return;
            HandleCutsceneFlow();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                _isPlaying = true;
                _iController.ChangeState(EntitiesState.Idle);
                
                SetupCollider(isEnable: false);
                CutsceneDirector.EnterCutscene();
                Debug.Log($"execute cutscene {cutsceneID}");
            }
        }
        
        // Initialize
        private void InitAttribute()
        {
            gameObject.name = cutsceneID;
            
            _currentIndex = 0;
            _isPlaying = false;
            _isTransitioning = false;
            _currentCutscene = cutsceneSteps[_currentIndex];
            
            var player = CutsceneManager.Instance.Player;
            _iController = player.GetComponent<IController>();
        }

        private void SetupCollider(bool isEnable)
        {
            _boxCollider2D.enabled = isEnable;
            _boxCollider2D.isTrigger = isEnable;
        }
        
        // Core
        private void HandleCutsceneFlow()
        {
            // Execute step
            if (!_hasExecutingStep)
            { 
                _currentCutscene.Execute();
                _hasExecutingStep = true;
            }
            
            // Check if finished
            if (_currentCutscene.IsFinished())
            {
                if (_currentIndex >= cutsceneSteps.Count - 1)
                {
                    _isPlaying = false;
                    _hasExecutingStep = false;
                    CutsceneDirector.ExitCutscene();
                    return;
                }
                
                StartCoroutine(TransitionToNextCutsceneRoutine());
            }
        }
        
        private IEnumerator TransitionToNextCutsceneRoutine()
        {
            _isTransitioning = true;
            yield return new WaitForSeconds(transitionTime);

            _currentIndex++;
            _isTransitioning = false;
            _hasExecutingStep = false;
            _currentCutscene = cutsceneSteps[_currentIndex];
        }
        
        #endregion
    }
}