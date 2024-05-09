using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Entities.Player;

namespace ATBMI.Interaction
{
    public class InteractArea : MonoBehaviour
    {
        #region Fields & Property

        [Header("Area Component")]
        [SerializeField] [Range(0, 5f)] private int areaCapacity;
        [SerializeField] private Vector2 areaSize;
        
        private bool _isAreaFilled;

        public BaseInteract InteractTarget { get; private set;}
        public bool IsInteracting { get; set; }

        // Reference
        public PlayerController PlayerController { get; private set; }
        public PlayerInputHandler PlayerInputHandler { get; private set; }
        public InteractEventHandler InteractEventHandler { get; private set;}

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PlayerController = transform.parent.GetComponent<PlayerController>();
            PlayerInputHandler = PlayerController.GetComponentInChildren<PlayerInputHandler>();
            InteractEventHandler = new InteractEventHandler();
        }

        private void Start()
        {
            _isAreaFilled = false;
            IsInteracting = false;
        }

        private void FixedUpdate()
        {
            InteractAreaChecker();
        }

        private void Update()
        {
            if (!_isAreaFilled || IsInteracting) return;
            if (PlayerInputHandler.IsPressInteract())
            {
                StartCoroutine(CallInteractOptionsRoutine());
            }
        }

        #endregion
        
        #region Methods 
        
        private void InteractAreaChecker()
        {
            // Alloc Overlapping
            Collider2D[] interactArea = new Collider2D[areaCapacity];
            var numOfHits = Physics2D.OverlapBoxNonAlloc(transform.position, areaSize, 0f, interactArea);

            foreach (var interactObject in interactArea)
            {
                if (interactObject == null) continue;
                if (interactObject.CompareTag("Interactable"))
                {
                    if (_isAreaFilled) return;
                    if (interactObject.TryGetComponent(out BaseInteract target))
                    {
                        InteractTarget = target;
                        _isAreaFilled = true;
                    }
                }
                else
                {
                    InteractTarget = null;
                    _isAreaFilled = false;
                }
            }
        }
        
        private IEnumerator CallInteractOptionsRoutine()
        {
            yield return new WaitForSeconds(0.1f);

            IsInteracting = true;
            PlayerController.StopMovement();
            InteractEventHandler.OpenInteractEvent();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, areaSize);
        }

        #endregion
    }
}
