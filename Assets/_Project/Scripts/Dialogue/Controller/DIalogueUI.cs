using System.Collections.Generic;
using ATBMI.Gameplay.Event;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private RectTransform parentRectTransform;
    [SerializeField] private int characterLimit;
    [SerializeField] private float horizontalOffset = 40f;

    private Dictionary<string, List<Transform>> npcTargets = new(); // cache the npc tip target for each npcs
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

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        DialogueEvents.RegisterNPCTipTarget += RegisterDialogueSignPoint;
        DialogueEvents.UpdateDialogueUIPos += UpdateDialogueUIPos;
        DialogueEvents.AdjustDialogueUISize += AdjustDialogueUISize;
        DialogueEvents.UnregisterDialogueSignPoint += UnregisterDialogueSignPoint;
    }

    private void UnsubscribeEvents()
    {
        DialogueEvents.RegisterNPCTipTarget -= RegisterDialogueSignPoint;
        DialogueEvents.UpdateDialogueUIPos -= UpdateDialogueUIPos;
        DialogueEvents.AdjustDialogueUISize -= AdjustDialogueUISize;
        DialogueEvents.UnregisterDialogueSignPoint -= UnregisterDialogueSignPoint;
    }

    private void Start()
    {
        corners = new Vector3[4];
        difference = 0;
        updateCounter = 0;
    }

    private void RegisterDialogueSignPoint(string npcName, Transform tipTarget)
    {
        if (!npcTargets.ContainsKey(npcName))
        {
            npcTargets[npcName] = new List<Transform>();
        }

        if (!npcTargets[npcName].Contains(tipTarget))
        {
            npcTargets[npcName].Add(tipTarget);
        }
    }

    private void UnregisterDialogueSignPoint()
    {
        npcTargets.Clear();
    }

    private void UpdateDialogueUIPos(string tagValue)
    {
        if (!npcTargets.TryGetValue(tagValue, out List<Transform> targets) || targets.Count == 0)
        {
            Debug.LogError($"NPC {tagValue} not found in registered targets!");
            return;
        }

        Transform closestTarget = null;
        float minSqrDistance = float.MaxValue;

        foreach (Transform target in targets)
        {
            float sqrDistance = (Camera.main.transform.position - target.position).sqrMagnitude;
            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                closestTarget = target;
            }
        }

        if (closestTarget != null)
        {
            pinPosition = closestTarget;
            screenPosition = Camera.main.WorldToScreenPoint(pinPosition.position);
            parentRectTransform.position = screenPosition;
        }
    }

    // TODO change method, so the background can detect the edge of screen, edge of image cannot surpass the edge of screen
    private void AdjustDialogueUISize(int charCounter)
    {
        layoutElement.enabled = charCounter > characterLimit;

        // play the update method for 10 frame
        updateCounter = 5;
    }

    private void LateUpdate()
    {
        if (screenPosition != null && pinPosition != null)
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
            difference = corners[2].x - Screen.width;
            rectTransform.position = new Vector3(rectTransform.position.x - difference - horizontalOffset, rectTransform.position.y, rectTransform.position.z);
        }
        else if (corners[0].x < 0)
        {
            difference = 0 - corners[0].x;
            rectTransform.position = new Vector3(rectTransform.position.x + difference + horizontalOffset, rectTransform.position.y, rectTransform.position.z);
        }

        updateCounter--;
    }


    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
}

