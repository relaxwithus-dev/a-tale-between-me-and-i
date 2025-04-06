using UnityEngine;
using TMPro;
using ATBMI.Core;
using ATBMI.Gameplay.Handler;
using UnityEngine.Serialization;

namespace ATBMI.Minigame
{
    public class MinigameTriggerer : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Attribute")] 
        [SerializeField] private MinigameView minigameView;
        [SerializeField] private TextMeshProUGUI infoTextUI;
        
        private bool _canPlayMinigame;
        
        // Reference
        private Collider2D _collider2D;

        #endregion

        #region Methods

        // Unity Callbacks
        private void Start()
        { 
            infoTextUI.gameObject.SetActive(false);
                
            _collider2D = GetComponent<Collider2D>();
            _collider2D.isTrigger = true;
            _collider2D.enabled = true;
        }
        
        private void Update()
        {
            if (!_canPlayMinigame) return;
            if (GameInputHandler.Instance.IsTapInteract)
            {
                _canPlayMinigame = false;
                
                infoTextUI.gameObject.SetActive(false);
                minigameView.EnterMinigame();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                _canPlayMinigame = true;
                infoTextUI.gameObject.SetActive(true);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                _canPlayMinigame = false;
                infoTextUI.gameObject.SetActive(false);
            }
        }
        
        #endregion
    }
}