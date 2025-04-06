using UnityEngine;
using ATBMI.Entities.Player;

namespace ATBMI.Minigame
{
    public class MinigameView : MonoBehaviour
    {
        #region Global Fields
        
        [Header("View")]
        [SerializeField] private GameObject minigamePanelUI;
        [SerializeField] private bool isPlayMinigame;
        
        protected const float MAX_SLIDER_VALUE = 1f;
        protected const float MIN_SLIDER_VALUE = 0f;
        
        // Reference
        private PlayerController _playerController;

        #endregion

        #region Methods

        // Unity Callbacks
        private void Awake()
        {
            _playerController = FindObjectOfType<PlayerController>();
        }
        
        private void Start()
        {
            InitOnStart();
        }
        
        private void Update()
        {
            if (!isPlayMinigame) return;
            RunMinigame();
        }
        
        // Core
        protected virtual void InitOnStart() { }
        
        public virtual void EnterMinigame()
        {
            isPlayMinigame = true;
            minigamePanelUI.SetActive(true);
            _playerController.StopMovement();
        }
        
        public virtual void ExitMinigame()
        {
            isPlayMinigame = false;
            minigamePanelUI.SetActive(false);
            _playerController.StartMovement();
        }
        
        protected virtual void RunMinigame() { }
        
        #endregion
    }
}