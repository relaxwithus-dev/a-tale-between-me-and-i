using UnityEngine;
using TMPro;
using ATBMI.Core;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Scene
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Traveler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Attribute")] 
        [SerializeField] private TextMeshProUGUI infoTextUI;
        private bool _canTravel;
        
        // Reference
        private Collider2D _collider2D;

        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Start()
        { 
            InitOnStart();
        }
        
        private void Update()
        {
            if (!_canTravel) return;
            if (DialogueManager.Instance.IsDialoguePlaying)
            {
                DisableTravel();
            }
            
            if (GameInputHandler.Instance.IsTapInteract)
            {
                TravelToTarget();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                EnableTravel();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                DisableTravel();
            }
        }
        
        // Initialize
        protected virtual void InitOnStart()
        {
            infoTextUI.gameObject.SetActive(false);
            
            _collider2D = GetComponent<Collider2D>();
            _collider2D.isTrigger = true;
            _collider2D.enabled = true;
        }
        
        // Core
        protected abstract void TravelToTarget();
        
        private void EnableTravel()
        {
            _canTravel = true;
            infoTextUI.gameObject.SetActive(true);
        }
        
        protected void DisableTravel()
        {
            _canTravel = false;
            infoTextUI.gameObject.SetActive(false);
        }
        
        #endregion
    }
}