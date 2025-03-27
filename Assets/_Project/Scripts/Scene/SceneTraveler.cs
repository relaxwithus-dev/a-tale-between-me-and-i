using UnityEngine;
using ATBMI.Core;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Scene
{
    [RequireComponent(typeof(Collider2D))]
    public class SceneTraveler : MonoBehaviour
    {
        [Header("Properties")] 
        [SerializeField] private string regionName;
        [SerializeField] private string sceneId;
        [SerializeField] private bool canTravel;
        
        // Reference
        private Collider2D _collider2D;
        
        // Methods
        private void Start()
        {
            _collider2D = GetComponent<Collider2D>();
            _collider2D.isTrigger = true;
            _collider2D.enabled = true;
        }
        
        private void Update()
        {
            if (!canTravel) return;
            if (GameInputHandler.Instance.IsTapInteract)
            {
                var sceneAsset = SceneDatabase.Instance.GetSceneAsset(regionName, sceneId);
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
                canTravel = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(GameTag.PLAYER_TAG))
            {
                canTravel = false;
            }
        }
    }
}