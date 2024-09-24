using ATBMI.Gameplay.Event;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoicesUI : MonoBehaviour
{
    [SerializeField] private RectTransform parentRectTransform;
    [SerializeField] private int characterLimit;

    private LayoutElement layoutElement;
    private RectTransform rectTransform;

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
        DialogEvents.UpdateDialogueChoicesUIPos += UpdateDialogueChoicesUIPos;
        DialogEvents.AdjustDialogueChoicesUISize += AdjustDialogueChoicesUISize;
    }

    private void OnDisable()
    {
        DialogEvents.UpdateDialogueChoicesUIPos -= UpdateDialogueChoicesUIPos;
        DialogEvents.AdjustDialogueChoicesUISize -= AdjustDialogueChoicesUISize;
    }

    private void Start()
    {
        corners = new Vector3[4];
        difference = 0;
        updateCounter = 0;
    }

    private void UpdateDialogueChoicesUIPos(string tagValue)
    {
        Transform targetPos = GameObject.FindGameObjectWithTag(tagValue).transform;
        Transform pinPosition = SearchPinPlaceholder(targetPos);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(pinPosition.position);

        if (screenPosition != null)
        {
            parentRectTransform.position = new Vector2(screenPosition.x, screenPosition.y);
        }
        else
        {
            Debug.LogError("Dialogue pin placeholder is missing!!");
        }
    }

    private Transform SearchPinPlaceholder(Transform targetPos)
    {
        foreach (Transform child in targetPos.transform)
        {
            if (child.CompareTag("DialoguePinPlaceholder"))
            {
                // If the child has the specific tag, add it to the list
                return child.transform;
            }
        }

        return null;
    }

    // TODO change method, so the background can detect the edge of screen, edge of image cannot surpass the edge of screen
    private void AdjustDialogueChoicesUISize(int charCounter)
    {
        layoutElement.enabled = charCounter > characterLimit;

        // play the update method for 10 frame
        updateCounter = 5;
    }

    private void Update()
    {
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
        else if (corners[0].x <= 0)
        {
            // Calculate the difference between the left edge and the screen width
            difference = 0 - corners[0].x;

            // Move the RectTransform left by the difference amount
            rectTransform.position = new Vector3(rectTransform.position.x + difference, rectTransform.position.y, rectTransform.position.z);
        }

        updateCounter--;
    }
}
