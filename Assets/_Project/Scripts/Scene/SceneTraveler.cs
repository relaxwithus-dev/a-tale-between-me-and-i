using UnityEngine;
using ATBMI.Core;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Scene
{
    [RequireComponent(typeof(Collider2D))]
    public class SceneTraveler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Attribute")] 
        [SerializeField] private string regionName;
        [SerializeField] private string sceneId;
        
        private bool _canTravel;
        
        // Reference
        private Collider2D _collider2D;

        #endregion

        #region Methods

        // Unity Callbacks
        private void Start()
        {
            _collider2D = GetComponent<Collider2D>();
            _collider2D.isTrigger = true;
            _collider2D.enabled = true;
        }
        
        private void Update()
        {
            if (!_canTravel) return;
            if (GameInputHandler.Instance.IsTapInteract)
            {
                var currentScene = SceneNavigation.Instance.CurrentScene;
                var sceneAsset = currentScene.GetNeighbourById(sceneId);

                if (!sceneAsset)
                {
                    Debug.LogWarning("target scene not found");
                    return;
                }
                
                SceneNavigation.Instance.SwitchScene(sceneAsset);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                _canTravel = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                _canTravel = false;
            }
        }
        
        #endregion
    }
}