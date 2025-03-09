using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private RectTransform parentRectTransform;
    [SerializeField] private int characterLimit;

    private Dictionary<string, Transform> npcTargets = new(); // cache the npc tip target for each npcs
    private LayoutElement layoutElement;
    private RectTransform rectTransform;

    private Transform pinPosition;
    private Vector3 screenPosition;

    private Vector3[] corners;
    private float difference;

    private int updateCounter;

    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        DialogueEvents.RegisterNPCTipTarget += RegisterNPCTipTarget;
        DialogueEvents.UpdateDialogueUIPos += UpdateDialogueUIPos;
        DialogueEvents.AdjustDialogueUISize += AdjustDialogueUISize;
    }

    private void OnDisable()
    {
        DialogueEvents.RegisterNPCTipTarget -= RegisterNPCTipTarget;
        DialogueEvents.UpdateDialogueUIPos -= UpdateDialogueUIPos;
        DialogueEvents.AdjustDialogueUISize -= AdjustDialogueUISize;
    }

    private void Start()
    {
        corners = new Vector3[4];
        difference = 0;
        updateCounter = 0;
    }

    private void RegisterNPCTipTarget(string npcName, Transform tipTarget)
    {
        if (!npcTargets.ContainsKey(npcName))
        {
            npcTargets[npcName] = tipTarget;
        }
    }

    private void UpdateDialogueUIPos(string tagValue)
    {
        if (npcTargets.TryGetValue(tagValue, out Transform targetPos))
        {
            pinPosition = targetPos;
            screenPosition = Camera.main.WorldToScreenPoint(targetPos.position);
            parentRectTransform.position = screenPosition;
        }
        else
        {
            Debug.LogError("NPC " + tagValue + " not found in registered targets!");
        }
    }

    // TODO change method, so the background can detect the edge of screen, edge of image cannot surpass the edge of screen
    private void AdjustDialogueUISize(int charCounter)
    {
        layoutElement.enabled = charCounter > characterLimit;

        // play the update method for 10 frame
        updateCounter = 5;
    }

    private void Update()
    {
        if (screenPosition != null)
        {
            parentRectTransform.position = Camera.main.WorldToScreenPoint(pinPosition.position);
        }

        if (updateCounter <= 0)
        {
            return;
        }

        rectTransform.position = parentRectTransform.position;
        rectTransform.GetWorldCorners(corners);

        // clamp right side
        if (corners[2].x >= Screen.width)
        {
            // Calculate the difference between the right edge and the screen width
            difference = corners[2].x - Screen.width;

            // Move the RectTransform left by the difference amount
            rectTransform.position = new Vector3(rectTransform.position.x - difference, rectTransform.position.y, rectTransform.position.z);
        }
        if (corners[0].x < 0)
        {
            // Calculate the difference between the left edge and the screen width
            difference = 0 - corners[0].x;

            // Move the RectTransform left by the difference amount
            rectTransform.position = new Vector3(rectTransform.position.x + difference, rectTransform.position.y, rectTransform.position.z);
        }

        updateCounter--;
    }
}

