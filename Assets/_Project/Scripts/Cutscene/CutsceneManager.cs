using UnityEngine;

namespace ATBMI.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        [Header("Attribute")] 
        [SerializeField] private CutsceneHandler[] cutsceneHandlers;
        
        private int _currentIndex;
        private bool _isCutscenePlaying;
        private CutsceneHandler _currentCutscene;
        
        private void Start()
        {
            InitCutscene();
        }

        private void InitCutscene()
        {
            // Stats
            _currentIndex = 0;
            _isCutscenePlaying = false;
            _currentCutscene = cutsceneHandlers[_currentIndex];
            
            // Handlers
            for (var i = 0; i < cutsceneHandlers.Length; i++)
            {
                var cutscene = cutsceneHandlers[i];
                var isFirstCutscene = i == _currentIndex;
                
                cutscene.CutsceneManager = this;
                cutscene.gameObject.SetActive(isFirstCutscene);
            }
        }
        
        public void EnterCutscene()
        {
            _currentCutscene.gameObject.SetActive(true);
            _isCutscenePlaying = true;
        }
        
        public void ExitCutscene()
        {
            _isCutscenePlaying = false;
            _currentCutscene.gameObject.SetActive(false);
            if (_currentIndex < cutsceneHandlers.Length - 1)
            {
                _currentIndex++;
                _currentCutscene = cutsceneHandlers[_currentIndex];
            }
        }
    }
}