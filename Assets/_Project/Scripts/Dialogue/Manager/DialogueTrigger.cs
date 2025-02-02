using UnityEngine;
using ATBMI.Entities.NPCs;
using ATBMI.Gameplay.Event;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Dialogue
{
    // will delete later, still in use for reference
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private Transform playerEntryPoint;

        // [Header("Ink JSON")]
        [HideInInspector] public TextAsset inkJSON;

        private VisualCue visualCue;
        private bool isVisualCueExists;
        private bool isDialogueAboutToStart;
        private bool isPlayerInRange;

        private CharacterAI npc;
        private PlayerDialogueHandler _playerDialogueHandler;

        private void Awake()
        {
            // TODO: change the method of getting this script
            _playerDialogueHandler = FindObjectOfType<PlayerDialogueHandler>();

            visualCue = GetComponentInChildren<VisualCue>();
            npc = transform.parent.gameObject.GetComponent<CharacterAI>();

            if (visualCue != null)
            {
                isVisualCueExists = true;
                visualCue.DeactivateVisualCue();
            }
            else
            {
                isVisualCueExists = false;
            }

            isPlayerInRange = false;
            isDialogueAboutToStart = false;
        }
        
        private void OnEnable()
        {
            DialogueEvents.OnEnterDialogue += OnEnterDialogue;
        }
        
        private void OnDisable()
        {
            DialogueEvents.OnEnterDialogue -= OnEnterDialogue;
        }
        
        private void Update()
        {
            if (isPlayerInRange && !DialogueManager.Instance.IsDialoguePlaying)
            {
                if (isVisualCueExists)
                {
                    visualCue.ActivateVisualCue();
                }

                if (GameInputHandler.Instance.IsTapInteract && !isDialogueAboutToStart)
                {
                    // playerController.MoveToDialogueEntryPoint(inkJSON, playerEntryPoint.position.x, npc.isFacingRight);
                    isDialogueAboutToStart = true;
                    Debug.Log(inkJSON.text);

                    // DialogueManager.Instance.EnterDialogueMode(inkJSON, GameObject.FindGameObjectWithTag("Player").transform, transform.parent);
                }
            }
            else
            {
                if (isVisualCueExists)
                {
                    visualCue.DeactivateVisualCue();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
            }
        }

        public void OnEnterDialogue()
        {
            // DialogueManager.Instance.EnterDialogueMode(InkJson);
            isDialogueAboutToStart = false;
        }
    }

}