using UnityEngine;
using UnityEngine.UI;
using ATBMI.Gameplay.Event;

namespace ATMBI.Dialogue
{
    public class DialogueChoiceHandler : MonoBehaviour
    {
        [SerializeField] private Transform playerSignTransform;
        [SerializeField] private RectTransform parentRectTransform;
        [SerializeField] private int characterLimit;
        
        private LayoutElement _layoutElement;
        private RectTransform _rectTransform;

        private Transform _pinPosition;
        private Vector3 _screenPosition;

        private Vector3[] _corners;
        private float _difference;

        private int _updateCounter;

        private void Awake()
        {
            _layoutElement = GetComponent<LayoutElement>();
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            DialogueEvents.UpdateDialogueChoicesUIPos += UpdateDialogueChoicesUIPos;
            DialogueEvents.AdjustDialogueChoicesUISize += AdjustDialogueChoicesUISize;
        }

        private void OnDisable()
        {
            DialogueEvents.UpdateDialogueChoicesUIPos -= UpdateDialogueChoicesUIPos;
            DialogueEvents.AdjustDialogueChoicesUISize -= AdjustDialogueChoicesUISize;
        }

        private void Start()
        {
            _corners = new Vector3[4];
            _difference = 0;
            _updateCounter = 0;
        }

        private void UpdateDialogueChoicesUIPos()
        {
            _pinPosition = playerSignTransform;
            _screenPosition = Camera.main.WorldToScreenPoint(_pinPosition.position);

            parentRectTransform.position = _screenPosition;
        }

        // TODO change method, so the background can detect the edge of screen, edge of image cannot surpass the edge of screen
        private void AdjustDialogueChoicesUISize(int charCounter)
        {
            _layoutElement.enabled = charCounter > characterLimit;

            // play the update method for 10 frame
            _updateCounter = 5;
        }

        private void LateUpdate()
        {
            if (_screenPosition != null)
            {
                parentRectTransform.position = Camera.main.WorldToScreenPoint(_pinPosition.position);
            }

            if (_updateCounter <= 0)
            {
                return;
            }

            _rectTransform.position = parentRectTransform.position;
            _rectTransform.GetWorldCorners(_corners);

            // clamp right side
            if (_corners[2].x >= Screen.width)
            {
                // Calculate the difference between the right edge and the screen width
                _difference = _corners[2].x - Screen.width;

                // Move the RectTransform left by the difference amount
                _rectTransform.position = new Vector3(_rectTransform.position.x - _difference, _rectTransform.position.y, _rectTransform.position.z);
            }
            else if (_corners[0].x <= 0)
            {
                // Calculate the difference between the left edge and the screen width
                _difference = 0 - _corners[0].x;

                // Move the RectTransform left by the difference amount
                _rectTransform.position = new Vector3(_rectTransform.position.x + _difference, _rectTransform.position.y, _rectTransform.position.z);
            }

            _updateCounter--;
        }
    }
}
