using System.Collections;
using System.Collections.Generic;
using ATBMI.Entities.Player;
using UnityEngine;

namespace ATBMI.Interaction
{
    public class InteractManager : MonoBehaviour
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject markerObject;

        // Reference
        private InteractController _interactController;
        private PlayerController _playerController;
        private PlayerInputHandler _playerInputHandler;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _interactController = player.GetComponentInChildren<InteractController>();
            _playerController = player.GetComponentInChildren<PlayerController>();
            _playerInputHandler = player.GetComponentInChildren<PlayerInputHandler>();
        }

        private void Start()
        {
            markerObject.SetActive(false);
        }

        private void Update()
        {
            if (!markerObject.activeSelf || _interactController.IsInteracting) return;
            if (_playerInputHandler.IsPressInteract())
            {
                StartCoroutine(CallInteractOptionsRoutine());
            }
        }

        #endregion

        #region Collider Callbacks

        private void OnTriggerEnter2D(Collider2D other)
        {
            HandleTrigger(other, true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            HandleTrigger(other, false);
        }

        #endregion

        #region Methods

        private IEnumerator CallInteractOptionsRoutine()
        {
            yield return new WaitForSeconds(0.1f);

            Debug.Log("go interact!");
            _interactController.IsInteracting = true;
            _playerController.StopMovement();
            InteractEventHandler.OpenInteractEvent();
        }

        private void HandleTrigger(Collider2D other, bool shouldEnter)
        {
            if (other.CompareTag("Player"))
            {
                var target = GetComponent<BaseInteract>();
                var container = _interactController.TargetContainer;

                markerObject.SetActive(shouldEnter);
                if (shouldEnter && !container.Contains(target))
                {
                    _interactController.TargetContainer.Add(target);
                }
                else if (!shouldEnter && container.Contains(target))
                {
                    _interactController.TargetContainer.Remove(target);
                }
            }
        }

        #endregion
        
    }
}
