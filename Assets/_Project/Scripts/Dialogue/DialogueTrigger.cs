using ATBMI.Entities.Player;
using UnityEngine;

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

    private NPC npc;
    private PlayerInputHandler playerInputHandler;
    private PlayerController playerController;

    private void Awake()
    {
        // TODO: change the method of getting this script
        playerInputHandler = FindObjectOfType<PlayerInputHandler>();
        playerController = FindObjectOfType<PlayerController>();

        visualCue = GetComponentInChildren<VisualCue>();
        npc = transform.parent.gameObject.GetComponent<NPC>();

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
        EventHandler.EnterDialogue += EnterDialogue;
    }

    private void OnDisable()
    {
        EventHandler.EnterDialogue -= EnterDialogue;
    }

    private void Update()
    {
        if (isPlayerInRange && !DialogueManager.Instance.isDialoguePlaying)
        {
            if (isVisualCueExists)
            {
                visualCue.ActivateVisualCue();
            }

            if (playerInputHandler.IsPressInteract() && !isDialogueAboutToStart)
            {
                playerController.MoveToDialogueEntryPoint(inkJSON, playerEntryPoint.position.x, npc.isFacingRight);
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

    public void EnterDialogue(TextAsset InkJson)
    {
        DialogueManager.Instance.EnterDialogueMode(InkJson);

        isDialogueAboutToStart = false;
    }
}
