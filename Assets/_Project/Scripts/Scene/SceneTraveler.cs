using UnityEngine;
using TMPro;
using ATBMI.Core;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Handler;
using UnityEngine.Serialization;

namespace ATBMI.Scene
{
    [RequireComponent(typeof(Collider2D))]
    public class SceneTraveler : MonoBehaviour
    {
        #region Fields & Properties
        
        [FormerlySerializedAs("locationTarget")]
        [Header("Attribute")] 
        [SerializeField] private LocationData locationData;
        [SerializeField] private TextMeshProUGUI infoTextUI;
        
        private bool _canTravel;
        
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
            if (!_canTravel) return;
            if (DialogueManager.Instance.IsDialoguePlaying)
            {
                DisableTravel();
            }
            
            if (GameInputHandler.Instance.IsTapInteract)
            {
                var currentScene = SceneNavigation.Instance.CurrentScene;
                var sceneAsset = currentScene.GetNeighbourById(locationData.location);

                if (!sceneAsset)
                {
                    Debug.LogWarning("target scene not found");
                    return;
                }
                
                infoTextUI.gameObject.SetActive(false);
                SceneNavigation.Instance.SwitchScene(sceneAsset);
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
        
        // Helper
        private void EnableTravel()
        {
            _canTravel = true;
            infoTextUI.gameObject.SetActive(true);
        }
        
        private void DisableTravel()
        {
            _canTravel = false;
            infoTextUI.gameObject.SetActive(false);
        }
        
        #endregion
    }
}