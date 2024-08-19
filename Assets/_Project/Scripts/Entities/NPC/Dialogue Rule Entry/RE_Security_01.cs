using ATBMI.Entities.Player;
using ATBMI.Gameplay.Event;
using ATBMI.Interaction;
using ATMBI.Gameplay.Event;
using UnityEngine;

public class RE_Security_01 : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private PlayerControllers playerController;
    [SerializeField] private Transform playerEntryPoint;
    private VisualCue visualCue;
    public bool isDialogueAboutToStart;
    private bool isVisualCueExists;
    private bool isPlayerInRange;
    private NPC npc;
    private InteractManager interactManager;

    [Space(20)]
    [Header("Dialogue Rules")]
    [SerializeField] private bool isAfterExplosion;
    [SerializeField] private bool isRunning;
    [SerializeField] private int visitedCount;
    [SerializeField] private bool isAfterGettingItem;


    [Space(20)]
    [Header("Dialogue Text Assets")]
    public TextAsset onTalk;

    [Space(10)]
    public TextAsset onTalkedTo_AfterExplosion_WithRunning;
    public bool isOnce_AfterExplosion_WithRunning;

    [Space(10)]
    public TextAsset onTalk_AfterExplosion_WithRunning_Visited_01;
    public bool isOnce_AfterExplosion_WithRunning_Visited_01;

    [Space(10)]
    public TextAsset onTalk_AfterExplosion_WithRunning_Visited_02;
    public bool isOnce_AfterExplosion_WithRunning_Visited_02;

    public TextAsset onTalk_AfterExplosion_WithRunning_Visited_03;
    public bool isOnce_AfterExplosion_WithRunning_Visited_03;

    [Space(10)]
    public TextAsset onTalk_AfterExplosion_WithWalking_Visited_01;
    public bool isOnce_AfterExplosion_WithWalking_Visited_01;

    [Space(10)]
    public TextAsset onTalk_AfterExplosion_WithWalking_Visited_02;
    public bool isOnce_AfterExplosion_WithWalking_Visited_02;

    public TextAsset onTalkedTo_AfterGettingAnItem;
    public bool isOnce_AfterGettingItem;

    [Space(10)]
    public TextAsset onTalk_AfterGettingAKey;
    public bool isOnce_AfterGettingKey;

    [Space(10)]
    public TextAsset onTalk_AfterGettingARock;
    public bool isOnce_AfterGettingRock;


    private bool isOnce01;
    private bool isOnce02;
    private bool isOnce03;
    private bool isOnce04;
    private bool isOnce05;
    private bool isOnce06;
    private bool isOnce07;
    private bool isOnce08;
    private bool isOnce09;

    private void Awake()
    {
        // TODO: change the method of getting this script
        interactManager = GetComponent<InteractManager>();

        visualCue = GetComponentInChildren<VisualCue>();
        npc = transform.parent.gameObject.GetComponent<NPC>();
    }

    private void OnEnable()
    {
        DialogEventHandler.EnterDialogue += EnterDialogue;
    }

    private void OnDisable()
    {
        DialogEventHandler.EnterDialogue -= EnterDialogue;
    }

    private void Start()
    {
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

        visitedCount = 0;
    }

    // private void Update()
    // {
    //     if (isPlayerInRange && !DialogueManager.Instance.isDialoguePlaying)
    //     {
    //         if (isVisualCueExists)
    //         {
    //             visualCue.ActivateVisualCue();
    //         }

    //         if (playerInputHandler.IsPressInteract() && !isDialogueAboutToStart)
    //         {
    //             bool isRuleMatch = false;

    //             if (isAfterExplosion && isRunning)
    //             {
    //                 visitedCount++;

    //                 if (visitedCount == 1 && !isOnce02)
    //                 {
    //                     PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithRunning_Visited_01, playerEntryPoint.position.x, npc.isFacingRight);

    //                     isRuleMatch = true;

    //                     if (isOnce_AfterExplosion_WithRunning_Visited_01)
    //                     {
    //                         isOnce02 = true;
    //                     }
    //                 }
    //                 else if (visitedCount == 2 && !isOnce03)
    //                 {
    //                     PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithRunning_Visited_02, playerEntryPoint.position.x, npc.isFacingRight);

    //                     isRuleMatch = true;

    //                     if (isOnce_AfterExplosion_WithRunning_Visited_02)
    //                     {
    //                         isOnce03 = true;
    //                     }
    //                 }
    //                 else if (visitedCount == 3 && !isOnce04)
    //                 {
    //                     PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithRunning_Visited_03, playerEntryPoint.position.x, npc.isFacingRight);

    //                     isRuleMatch = true;

    //                     if (isOnce_AfterExplosion_WithRunning_Visited_03)
    //                     {
    //                         isOnce04 = true;
    //                     }
    //                 }
    //             }

    //             if (isAfterExplosion && !isRunning)
    //             {
    //                 visitedCount++;

    //                 if (visitedCount == 1 && !isOnce05)
    //                 {
    //                     PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithWalking_Visited_01, playerEntryPoint.position.x, npc.isFacingRight);

    //                     isRuleMatch = true;

    //                     if (isOnce_AfterExplosion_WithWalking_Visited_01)
    //                     {
    //                         isOnce05 = true;
    //                     }
    //                 }
    //                 else if (visitedCount == 2 && !isOnce06)
    //                 {
    //                     PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithWalking_Visited_02, playerEntryPoint.position.x, npc.isFacingRight);

    //                     isRuleMatch = true;

    //                     if (isOnce_AfterExplosion_WithWalking_Visited_02)
    //                     {
    //                         isOnce06 = true;
    //                     }
    //                 }
    //             }

    //             // if (isAfterGettingItem)
    //             // {
    //             //     // TODO: make inventory manager

    //             //     // if (InventoryManager.Instance.LastItem.itemName == "Key")
    //             //     // {
    //             //     //     DialogueManager.Instance.EnterDialogueMode(onTalkAfterGettingAKey);
    //             //     // }
    //             //     // else if(InventoryManager.Instance.LastItem.itemName == "Rock")
    //             //     // {
    //             //     //     DialogueManager.Instance.EnterDialogueMode(onTalkAfterGettingARock);
    //             //     // }
    //             //     Debug.Log("Getting Item");

    //             //     isRuleMatch = true;
    //             // }

    //             if (!isRuleMatch)
    //             {
    //                 PlayerEventHandler.MoveToPlayerEvent(onTalk, playerEntryPoint.position.x, npc.isFacingRight);
    //             }

    //             isDialogueAboutToStart = true;
    //         }
    //     }
    //     else
    //     {
    //         if (isVisualCueExists)
    //         {
    //             visualCue.DeactivateVisualCue();
    //         }
    //     }
    // }

    // if dialogue trigger after player entering object/npc area
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (isAfterExplosion && isRunning && !isOnce01)
            {
                DialogueManager.Instance.EnterDialogueMode(onTalkedTo_AfterExplosion_WithRunning);

                if (isOnce_AfterExplosion_WithRunning)
                {
                    isOnce01 = true;
                }
            }

            if (isAfterGettingItem && isRunning && !isOnce07)
            {
                DialogueManager.Instance.EnterDialogueMode(onTalkedTo_AfterGettingAnItem);

                if (isOnce_AfterGettingItem)
                {
                    isOnce07 = true;
                }
            }
        }
    }

    // if dialogue trigger after player exiting object/npc area
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

    public void TestRE()
    {
        if (!isDialogueAboutToStart)
        {
            bool isRuleMatch = false;

            if (isAfterExplosion && isRunning)
            {
                visitedCount++;

                if (visitedCount == 1 && !isOnce02)
                {
                    PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithRunning_Visited_01, playerEntryPoint.position.x, npc.isFacingRight);

                    isRuleMatch = true;

                    if (isOnce_AfterExplosion_WithRunning_Visited_01)
                    {
                        isOnce02 = true;
                    }
                }
                else if (visitedCount == 2 && !isOnce03)
                {
                    PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithRunning_Visited_02, playerEntryPoint.position.x, npc.isFacingRight);

                    isRuleMatch = true;

                    if (isOnce_AfterExplosion_WithRunning_Visited_02)
                    {
                        isOnce03 = true;
                    }
                }
                else if (visitedCount == 3 && !isOnce04)
                {
                    PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithRunning_Visited_03, playerEntryPoint.position.x, npc.isFacingRight);

                    isRuleMatch = true;

                    if (isOnce_AfterExplosion_WithRunning_Visited_03)
                    {
                        isOnce04 = true;
                    }
                }
            }

            if (isAfterExplosion && !isRunning)
            {
                visitedCount++;

                if (visitedCount == 1 && !isOnce05)
                {
                    PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithWalking_Visited_01, playerEntryPoint.position.x, npc.isFacingRight);

                    isRuleMatch = true;

                    if (isOnce_AfterExplosion_WithWalking_Visited_01)
                    {
                        isOnce05 = true;
                    }
                }
                else if (visitedCount == 2 && !isOnce06)
                {
                    PlayerEventHandler.MoveToPlayerEvent(onTalk_AfterExplosion_WithWalking_Visited_02, playerEntryPoint.position.x, npc.isFacingRight);

                    isRuleMatch = true;

                    if (isOnce_AfterExplosion_WithWalking_Visited_02)
                    {
                        isOnce06 = true;
                    }
                }
            }

            // if (isAfterGettingItem)
            // {
            //     // TODO: make inventory manager

            //     // if (InventoryManager.Instance.LastItem.itemName == "Key")
            //     // {
            //     //     DialogueManager.Instance.EnterDialogueMode(onTalkAfterGettingAKey);
            //     // }
            //     // else if(InventoryManager.Instance.LastItem.itemName == "Rock")
            //     // {
            //     //     DialogueManager.Instance.EnterDialogueMode(onTalkAfterGettingARock);
            //     // }
            //     Debug.Log("Getting Item");

            //     isRuleMatch = true;
            // }

            if (!isRuleMatch)
            {
                PlayerEventHandler.MoveToPlayerEvent(onTalk, playerEntryPoint.position.x, npc.isFacingRight);
            }

            isDialogueAboutToStart = true;
        }
    }
}
