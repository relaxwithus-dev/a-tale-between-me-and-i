using UnityEngine;
using TMPro;
using ATBMI.Core;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Minigame
{
    [RequireComponent(typeof(Collider2D))]
    public class MinigameTriggerer : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Attribute")] 
        [SerializeField] private bool isActivateEarly;
        [SerializeField] private TextMeshProUGUI infoTextUI;
        [SerializeField] private MinigameManager minigameManager;
        
        private bool _canPlayMinigame;
        
        // Reference
        private Collider2D _collider2D;

        #endregion

        #region Methods

        // Unity Callbacks
        private void OnEnable()
        {
            MinigameEvents.OnActivateTrigger += ActivateTrigger;
            MinigameEvents.OnWinMinigame += DeactivateTrigger;
        }

        private void OnDisable()
        {
            MinigameEvents.OnActivateTrigger -= ActivateTrigger;
            MinigameEvents.OnWinMinigame -= DeactivateTrigger;
        }
        
        private void Start()
        {
            InitTriggerer();
        }
        
        private void Update()
        {
            if (!_canPlayMinigame) return;
            
            if (DialogueManager.Instance.IsDialoguePlaying || minigameManager.IsPlayingMinigame)
                DisableTravel();
            
            if (GameInputHandler.Instance.IsTapInteract)
            {
                _canPlayMinigame = false;
                
                infoTextUI.gameObject.SetActive(false);
                MinigameEvents.EnterMinigameEvent();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActivateEarly)
                return;
                
            if (other.CompareTag(GameTag.PLAYER_TAG) && isActivateEarly)
            {
                EnableTravel();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!isActivateEarly)
                return;
            
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                DisableTravel();
            }
        }
        
        // Initialize
        private void InitTriggerer()
        {
            _collider2D = GetComponent<Collider2D>();
            _collider2D.isTrigger = true;
            _collider2D.enabled = true;
            
            infoTextUI.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void ActivateTrigger()
        {
            InitTriggerer();
            isActivateEarly = true;
        }

        private void DeactivateTrigger()
        {
            isActivateEarly = false;
            _collider2D.enabled = false;
            gameObject.SetActive(false);
        }
        
        // Helpers
        private void EnableTravel()
        {
            _canPlayMinigame = true;
            infoTextUI.gameObject.SetActive(true);
        }
        
        private void DisableTravel()
        {
            _canPlayMinigame = false;
            infoTextUI.gameObject.SetActive(false);
        }
        
        #endregion
    }
}