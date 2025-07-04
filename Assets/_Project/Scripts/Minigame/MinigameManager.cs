using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        #region Fields & Properties
        
        private enum MinigameType { Arrow, Timing, Mash }

        [Header("Properties")] 
        [SerializeField] private bool isDebugMode;
        [SerializeField] private MinigameType minigameType;
        [SerializeField] [ReadOnly] private MinigameView[] minigameViews;
        
        private MinigameView _selectedView;
        public bool IsPlayingMinigame { get; private set; }
        
        // Reference
        private PlayerController _playerController;
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            _playerController = FindObjectOfType<PlayerController>();
        }

        private void OnEnable()
        {
            MinigameEvents.OnEnterMinigame += EnterEnterMinigame;
        }
        
        private void OnDisable()
        {
            MinigameEvents.OnEnterMinigame -= EnterEnterMinigame;
        }

        private void Start()
        {
            _selectedView = GetMinigameView(minigameType);
            foreach (var view in minigameViews)
            {
                view.gameObject.SetActive(false);
            }
        }
        
        private void Update()
        {
            if (!isDebugMode) return;
            if (GameInputHandler.Instance.IsTapInteract && !IsPlayingMinigame)
            {
                EnterEnterMinigame();
            }
        }
        
        // Core
        private void EnterEnterMinigame()
        {
            _playerController.StopMovement();
            _selectedView.gameObject.SetActive(true);
            _selectedView.EnterMinigame();
            
            IsPlayingMinigame = true;
            StressEvents.StressOnceEvent(isAddStress: false);
        }
        
        public void ExitMinigame()
        {
            if (!_selectedView)
                return;
            
            IsPlayingMinigame = false;
            _playerController.StartMovement();
            _selectedView.gameObject.SetActive(false);
        }
        
        private MinigameView GetMinigameView(MinigameType type)
        {
            return type switch
            {
                MinigameType.Arrow => minigameViews[0],
                MinigameType.Timing => minigameViews[1],
                MinigameType.Mash => minigameViews[2],
                _ => null
            };
        }
        
        #endregion
    }
}