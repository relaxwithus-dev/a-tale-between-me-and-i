using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Entities.Player;

namespace ATBMI.Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        #region Fields & Properties
        
        private enum MinigameType { Arrow, Balance, Mash }
        
        [Header("Properties")]
        [SerializeField] private MinigameType minigameType;
        [SerializeField] [ReadOnly] private MinigameView[] minigameViews;
        
        private MinigameView _selectedView;
        
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
            _selectedView = GetMinigameView(minigameType);
            foreach (var view in minigameViews)
            {
                view.gameObject.SetActive(false);
            }
        }
        
        // TODO: Drop update method pas minigame aman
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EnterMinigame();
            }
        }
        
        // Core
        public void EnterMinigame()
        {
            _playerController.StopMovement();
            _selectedView.gameObject.SetActive(true);
            _selectedView.EnterMinigame();
        }
        
        public void ExitMinigame()
        {
            if (!_selectedView)
                return;
            
            _playerController.StartMovement();
            _selectedView.gameObject.SetActive(false);
        }

        private MinigameView GetMinigameView(MinigameType type)
        {
            return type switch
            {
                MinigameType.Arrow => minigameViews[0],
                MinigameType.Balance => minigameViews[1],
                MinigameType.Mash => minigameViews[2],
                _ => null
            };
        }
        
        #endregion
    }
}