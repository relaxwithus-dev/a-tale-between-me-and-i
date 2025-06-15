using UnityEngine;

namespace ATBMI.Gameplay.Controller
{
    public class ParallaxController : MonoBehaviour
    {
        [Header("Requirements")]
        [Tooltip("0 = move with camera, 1 = no movement")]
        [SerializeField] [Range(0, 1f)] private float parallaxAmountX;
        [SerializeField] [Range(0, 1f)] private float parallaxAmountY;
        [SerializeField] private bool isLoop;
        [SerializeField] private Vector2 camStartPos;
        
        private Vector2 _startPos;
        private float _boundLength;
        
        // Reference
        private Camera _mainCamera;
        private SpriteRenderer _spriteRenderer;
        
        // Methods
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            _startPos = transform.position;
            _boundLength = _spriteRenderer.localBounds.size.x;
        }

        private void Update()
        {
            var distanceX = (_mainCamera.transform.position.x - camStartPos.x) * parallaxAmountX;
            var distanceY = (_mainCamera.transform.position.y - camStartPos.y) * parallaxAmountY;

            transform.position = new Vector2(_startPos.x + distanceX, _startPos.y + distanceY);

            var movementX = (_mainCamera.transform.position.x - camStartPos.x) * (1 - parallaxAmountX);
            
            if (!isLoop) return;
            if (movementX > _startPos.x + _boundLength)
                _startPos.x += _boundLength;
            else if (movementX < _startPos.x - _boundLength)
                _startPos.x -= _boundLength;
        }
    }
}
