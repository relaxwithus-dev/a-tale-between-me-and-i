using System.Collections;
using UnityEngine;
using ATBMI.Player;
using ATBMI.Gameplay.Handler;
using ATBMI.Gameplay.Event;

namespace ATBMI.Interaction
{
    public class InteractzManager : MonoBehaviour
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject markerObject;

        // Reference
        private InteractController _interactController;
        private PlayerController _playerController;

        private bool isDialogueAboutToStart;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerController = player.GetComponent<PlayerController>();
            _interactController = player.GetComponentInChildren<InteractController>();
        }

        private void Start()
        {
            markerObject.SetActive(false);
        }

        private void Update()
        {
            if (!markerObject.activeSelf || _interactController.IsInteracting) return;
            if (GameInputHandler.Instance.IsTapInteract && !DialogueManager.Instance.isDialoguePlaying)
            {
                StartCoroutine(CallInteractOption());
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

        private IEnumerator CallInteractOption()
        {
            _playerController.StopMovement();
            PlayerEvents.InteractEvent();

            yield return new WaitForSeconds(0.1f);
            _interactController.IsInteracting = true;
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
