using System.Collections;
using UnityEngine;
using ATBMI.Gameplay.Event;

namespace ATBMI.Player
{
    public class PlayerDialogHandler : MonoBehaviour
    {
        #region Fields & Properties

        private TextAsset _playerInkJson;
        private PlayerController _playerController;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            PlayerEventHandler.OnMoveToPlayer += MoveToDialogueEntryPoint;
        }

        private void OnDisable()
        {
            PlayerEventHandler.OnMoveToPlayer -= MoveToDialogueEntryPoint;
        }

        #endregion

        #region Methods
        
        public void MoveToDialogueEntryPoint(TextAsset INKJson, float newPositionX, bool isNpcFacingRight)
        {
            _playerInkJson = INKJson;
            FlipPlayerWhenDialogue(newPositionX, isNpcFacingRight);

            // TODO: change to dotween?
            StartCoroutine(MoveToPointRoutine(newPositionX, isNpcFacingRight));
        }
        
        private IEnumerator MoveToPointRoutine(float newPositionX, bool isNpcFacingRight)
        {
            var initialPosition = transform.position;
            var targetPosition = new Vector3(newPositionX, initialPosition.y, initialPosition.z);

            // Calculate the distance manually
            float distance = Mathf.Abs(targetPosition.x - initialPosition.x);
            float speed = _playerController.CurrentSpeed / 2 / distance; 

            var progress = 0f;
            while (progress <= 1f)
            {
                progress += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(initialPosition, targetPosition, progress);
                yield return null;
            }

            // Ensure the final position is exactly newPosition
            transform.position = targetPosition;

            if (FlipPlayerWhenDialogue(newPositionX, isNpcFacingRight))
            {
                yield return new WaitForSeconds(0.2f);
            }

            DialogEventHandler.EnterDialogueEvent(_playerInkJson);
            _playerInkJson = null;
        }

        private bool FlipPlayerWhenDialogue(float newPositionX, bool isNpcFacingRight)
        {
            if (transform.position.x < newPositionX)
            {
                if (_playerController.IsRight && !isNpcFacingRight)
                {
                    _playerController.PlayerFlip();
                    return true;
                }
                else if (_playerController.IsRight && isNpcFacingRight)
                {
                    _playerController.PlayerFlip();
                    return true;
                }
            }
            else if (transform.position.x > newPositionX)
            {
                if (_playerController.IsRight && !isNpcFacingRight)
                {
                    _playerController.PlayerFlip();
                    return true;
                }
                else if (_playerController.IsRight && isNpcFacingRight)
                {
                    _playerController.PlayerFlip();
                    return true;
                }
            }
            else if (transform.position.x == newPositionX)
            {
                if (_playerController.IsRight && !isNpcFacingRight)
                {
                    _playerController.PlayerFlip();
                    return true;
                }
                else if (_playerController.IsRight && isNpcFacingRight)
                {
                    _playerController.PlayerFlip();
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}