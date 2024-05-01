using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBMI.Interaction;

namespace ATBMI.Entities.Player
{
    public class InteractArea : MonoBehaviour
    {
        #region Fields & Property

        [Header("Area Component")]
        [SerializeField] [Range(0, 5f)] private int areaCapacity;
        [SerializeField] private Vector2 areaSize;
        
        private bool _isAreaFilled;

        public InteractionBase InteractionBase { get; private set;}
        public bool IsInteracting { get; set; }

        // Event
        public event Action<InteractionBase> OnInteractTriggered; 

        [Header("Reference")]
        [SerializeField] private PlayerInputHandler playerInputHandler;

        #endregion

        #region MonoBehaviour Callbacks

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
            if (!_isAreaFilled) return;
            if (IsInteracting) return;

            if (playerInputHandler.InteractTriggered)
            {
                Debug.LogWarning("call optioin");
                CallInteractOptions();
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
                    if (interactObject.TryGetComponent(out InteractionBase target))
                    {
                        InteractionBase = target;
                        _isAreaFilled = true;
                    }
                }
                else
                {
                    InteractionBase = null;
                    _isAreaFilled = false;
                }
            }
        }

        private void CallInteractOptions()
        {
            OnInteractTriggered?.Invoke(InteractionBase);
            IsInteracting = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, areaSize);
        }

        #endregion
    }
}
