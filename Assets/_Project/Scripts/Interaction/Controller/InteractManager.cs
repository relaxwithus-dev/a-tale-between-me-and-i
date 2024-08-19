using System.Collections;
using UnityEngine;
using ATMBI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Event;
using System.Threading;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Interaction
{
    public class InteractManager : MonoBehaviour
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject markerObject;

        // Reference
        private InteractController _interactController;
        private PlayerControllers _playerController;

        private bool isDialogueAboutToStart;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerController = player.GetComponentInChildren<PlayerControllers>();
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
            PlayerEventHandler.InteractEvent();

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
