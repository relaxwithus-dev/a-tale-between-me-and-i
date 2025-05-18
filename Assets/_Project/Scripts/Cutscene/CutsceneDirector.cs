using UnityEngine;

namespace ATBMI.Cutscene
{
    public class CutsceneDirector : MonoBehaviour
    {
        #region Fields
        
        [Header("Attribute")] 
        [SerializeField] private CutsceneHandler[] cutsceneHandlers;
        private CutsceneHandler _currentCutscene;
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void OnEnable()
        {
            ModifyHandlers();
            CutsceneManager.OnCutsceneEnd += ModifyHandlers;
        }
        
        private void OnDisable()
        {
            CutsceneManager.OnCutsceneEnd -= ModifyHandlers;
        }
        
        // Core
        public void EnterCutscene(CutsceneHandler handler)
        {
            _currentCutscene = handler;
            CutsceneManager.Instance.EnterCutscene();
        }
        
        public void ExitCutscene()
        {
            _currentCutscene.gameObject.SetActive(false);
            CutsceneManager.Instance.ExitCutscene();
        }
        
        private void ModifyHandlers()
        {
            var key = CutsceneManager.Instance.CurrentKeys;
            foreach (var handler in cutsceneHandlers)
            {
                var isActiveCutscene = key && handler.CutsceneKey.id == key.id;
                
                Debug.Log(isActiveCutscene);
                handler.CutsceneDirector ??= this;
                handler.gameObject.SetActive(isActiveCutscene);
            }
        }
        
        #endregion
    }
}