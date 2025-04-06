using UnityEngine;
using ATBMI.Entities.Player;

namespace ATBMI.Minigame
{
    public class MinigameManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Properties")]
        [SerializeField] private bool isPlaying;
        [SerializeField] private MinigameView minigameView;
        
        // Reference
        private PlayerController _playerController;
        
        #endregion
        
        #region Methods
        
        // Unity Callbacks
        private void Awake()
        {
            _playerController = FindObjectOfType<PlayerController>();
        }
        
        // Core
        public void EnterMinigame()
        {
            _playerController.StopMovement();
            minigameView.gameObject.SetActive(true);
            minigameView.EnterMinigame();
        }
        
        public void ExitMinigame()
        {
            _playerController.StopMovement();
        }
        
        #endregion
    }
}