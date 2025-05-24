using UnityEngine;
using ATBMI.Entities.Player;

namespace ATBMI.Cutscene
{
    public class CutsceneDirector : MonoBehaviour
    {
        #region Fields
        
        [Header("Attribute")] 
        [SerializeField] private CutsceneHandler[] cutsceneHandlers;
        
        private CutsceneHandler _currentCutscene;
        private PlayerController _player;
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void OnEnable()
        {
            CutsceneManager.OnModifyCutscene += ModifyHandlers;
        }
        
        private void OnDisable()
        {
            CutsceneManager.OnModifyCutscene -= ModifyHandlers;
        }

        private void Start()
        {
            // Init player reference
            var player = CutsceneManager.Instance.Player;
            _player = player.GetComponent<PlayerController>();
            
            // Init handlers
            ModifyHandlers();
        }
        
        // Core
        public void EnterCutscene(CutsceneHandler handler)
        {
            _player.StopMovement();
            _currentCutscene = handler;
            CutsceneManager.Instance.EnterCutscene();
        }
        
        public void ExitCutscene()
        {
            _player.StartMovement();
            _currentCutscene.gameObject.SetActive(false);
            CutsceneManager.Instance.ExitCutscene();
        }
        
        private void ModifyHandlers()
        {
            var key = CutsceneManager.Instance.CurrentKeys;
            foreach (var handler in cutsceneHandlers)
            {
                var isActiveCutscene = key && handler.CutsceneKey.id == key.id;
                
                handler.CutsceneDirector ??= this;
                handler.gameObject.SetActive(isActiveCutscene);
            }
        }
        
        #endregion
    }
}