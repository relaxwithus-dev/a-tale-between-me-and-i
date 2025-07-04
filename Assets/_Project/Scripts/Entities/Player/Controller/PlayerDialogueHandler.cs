using System.Collections;
using UnityEngine;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Event;

namespace ATBMI.Entities.Player
{
    public class PlayerDialogueHandler : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Reference")]
        [SerializeField] private Transform signTransform;
        [SerializeField] private Transform emojiTransform;

        private TextAsset _playerInkJson;
        private PlayerController _playerController;

        #endregion

        #region Methods

        // Unity Callbacks
        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            DialogueEvents.RegisterDialogueSignPoint += RegisterDialogueSignPoint;
            PlayerEvents.OnMoveToPlayer += MoveToDialogueEntryPoint;
        }

        private void OnDisable()
        {
            DialogueEvents.RegisterDialogueSignPoint -= RegisterDialogueSignPoint;
            PlayerEvents.OnMoveToPlayer -= MoveToDialogueEntryPoint;
        }
        
        // Core
        private void RegisterDialogueSignPoint()
        {
            DialogueEvents.RegisterNPCTipTargetEvent(_playerController.Data.name, signTransform);
            DialogueEvents.RegisterNPCEmojiTargetEvent(_playerController.Data.name, emojiTransform);
        }
        
        private void MoveToDialogueEntryPoint(RuleEntry rule, TextAsset ink, float newPositionX, float npcPosX, bool isNpcFacingRight)
        {
            _playerInkJson = ink;
            if (ShouldFlipPlayerWhenDialogue(newPositionX, npcPosX, isNpcFacingRight))
            {
                _playerController.Flip();
            }

            // TODO: change to dotween?
            StartCoroutine(MoveToPointRoutine(rule, newPositionX, npcPosX, isNpcFacingRight));
        }

        private IEnumerator MoveToPointRoutine(RuleEntry rule, float newPositionX, float npcPosX, bool isNpcFacingRight)
        {
            _playerController.StopMovement();

            var initialPosition = transform.position;
            var targetPosition = new Vector3(newPositionX, initialPosition.y, initialPosition.z);

            // Loop until the player reaches the target position
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                // Calculate the direction to the target
                var directionToTarget = (targetPosition - transform.position).normalized;

                // Simulate input for the PlayerMove method
                _playerController.SetTemporaryDirection(new Vector2(directionToTarget.x, 0));

                // Wait for FixedUpdate to process the movement
                yield return new WaitForFixedUpdate();
            }

            // Stop movement once the target is reached
            _playerController.SetTemporaryDirection(Vector2.zero);
            yield return new WaitForFixedUpdate();

            // Ensure the final position is exactly the target position
            transform.position = targetPosition;

            if (ShouldFlipPlayerWhenDialogue(newPositionX, npcPosX, isNpcFacingRight))
            {
                yield return new WaitForSeconds(0.05f);
                _playerController.Flip();
            }

            rule.EnterDialogueWithInkJson(_playerInkJson);
            _playerInkJson = null;

            _playerController.StartMovement();
        }


        private bool ShouldFlipPlayerWhenDialogue(float newPlayerPosX, float npcPosX, bool isNpcFacingRight)
        {
            // Determine if the player is currently on the left or right of the NPC
            bool playerOnLeftOfNpc = transform.position.x < npcPosX;

            // Check if the player's new position is stationary
            bool isPlayerStationary = Mathf.Approximately(transform.position.x, newPlayerPosX);

            // Determine if the player is moving to the left or right of their new position
            bool playerMovingLeft = newPlayerPosX < transform.position.x;
            bool playerMovingRight = newPlayerPosX > transform.position.x;

            // Determine if the NPC should flip
            bool shouldFlip = false;
            
            if (isPlayerStationary)
            {
                // If the player is stationary, the NPC should face the player
                shouldFlip = playerOnLeftOfNpc != isNpcFacingRight;
            }
            else
            {
                // Check movement scenarios
                if (playerMovingRight)
                {
                    // Player moving right
                    shouldFlip = (!_playerController.IsFacingRight && isNpcFacingRight) // Player facing left, NPC faces right
                                || (playerOnLeftOfNpc && !isNpcFacingRight);      // Player on left, NPC facing left
                }
                else if (playerMovingLeft)
                {
                    // Player moving left
                    shouldFlip = (_playerController.IsFacingRight && !isNpcFacingRight) // Player facing right, NPC faces left
                                || (!playerOnLeftOfNpc && isNpcFacingRight);      // Player on right, NPC facing right
                }
            }

            return shouldFlip;
        }

        #endregion
    }
}