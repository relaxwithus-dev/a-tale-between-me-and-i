using UnityEngine;
using ATBMI.Core;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Gameplay.Controller
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TravelController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Require")]
        [SerializeField] private string targetMapName; 
        [SerializeField] private GameObject notifierUI;
        private bool _canTravel;
        
        [Header("Reference")]
        [SerializeField] private MapLoader mapLoader;

        #endregion

        #region Methods

        // Unity Callbacks
        private void Start()
        {
            notifierUI.SetActive(false);
        }

        private void Update()
        {
            if (!_canTravel) return;
            if (GameInputHandler.Instance.IsTapInteract)
            {
                mapLoader.LoadMapAsync(targetMapName);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                _canTravel = true;
                if (notifierUI != null)
                    notifierUI.SetActive(true);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                _canTravel = false;
                if (notifierUI != null)
                    notifierUI.SetActive(false);
            }
        }

        #endregion
    }
}