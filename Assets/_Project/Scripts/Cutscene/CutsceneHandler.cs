using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Core;
using ATBMI.Dialogue;

namespace ATBMI.Cutscene
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CutsceneHandler : MonoBehaviour
    {
        #region Method
        
        [Header("Attribute")]
        [SerializeField] private bool hasConditionToPlay;
        [SerializeField] private float transitionTime;
        [SerializeField] private CutsceneKeys cutsceneKey;
        [SerializeField] private List<Cutscene> cutsceneSteps;
        
        private int _currentIndex;
        private bool _isPlaying;
        private bool _isTransitioning;
        private bool _hasExecutingStep;
        
        private Cutscene _currentCutscene;
        public CutsceneKeys CutsceneKey => cutsceneKey;
        public bool HasConditionToPlay
        {
            get => hasConditionToPlay;
            set => hasConditionToPlay = value;
        }
        
        // Reference
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
            if (DialogueManager.Instance.IsDialoguePlaying)
                return;
            
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                _isPlaying = true;
                
                SetupCollider(isEnable: false);
                CutsceneDirector.EnterCutscene(this);
            }
        }
        
        // Initialize
        private void InitAttribute()
        {
            // Stats
            _currentIndex = 0;
            _isPlaying = false;
            _isTransitioning = false;
            _currentCutscene = cutsceneSteps[_currentIndex];
            
            // Steps
            foreach (var step in cutsceneSteps)
            {
                var isActive = step == _currentCutscene;
                step.gameObject.SetActive(isActive);
            }
        }
        
        private void SetupCollider(bool isEnable)
        {
            _boxCollider2D.enabled = isEnable;
            _boxCollider2D.isTrigger = isEnable;
        }
        
        // Core
        public void EnterDirectCutscene()
        {
            _isPlaying = true;
            SetupCollider(isEnable: false);
            CutsceneDirector.EnterCutscene(this);
        }
        
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
            _currentCutscene.gameObject.SetActive(true);
        }
        
        #endregion
    }
}