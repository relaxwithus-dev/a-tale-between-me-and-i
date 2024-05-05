using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;
using ATBMI.Entities.Player;
using ATBMI.Enum;

namespace ATBMI.Interaction
{
    public class InteractOptions : MonoBehaviour
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject interactOptionsUI;
        [SerializeField] private TextMeshProUGUI optionInfoTextUI;

        [SerializeField] private List<Button> _optionButtons;

        [Header("References")]
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;
        private InteractArea _interactArea;
        private PlayerInputHandler _playerInputHandler;
        private InteractEventHandler _interactEventHandler;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _interactArea = GetComponent<InteractArea>();
        }

        private void OnEnable()
        {
            // Inject Variables
            _playerInputHandler = _interactArea.PlayerInputHandler;
            _interactEventHandler = _interactArea.InteractEventHandler;

            // Event
            _interactEventHandler.OnOpenInteract += OnInteractTriggered;
        }
        
        private void OnDisable()
        {
            // Event
            _interactEventHandler.OnOpenInteract -= OnInteractTriggered;
        }

        private void Start()
        {
            interactOptionsUI.SetActive(false);
        }

        private void Update()
        {
            if (!_interactArea.IsInteracting) return;

            HandleNavigation();
            HandleInteraction();
        }

        #endregion

        #region Methods

        // !-- Core Functionality
        private void OnInteractTriggered()
        {
            InitilizeButtons();
            interactOptionsUI.SetActive(true);
        }

        // !-- Button Fields
        private void InitilizeButtons()
        {
            var optionsCount = interactOptionsUI.transform.childCount;
            _optionButtons = new List<Button>(optionsCount);
            for (var i = 0; i < optionsCount; i++)
            {
                var buttonParent = interactOptionsUI.transform.GetChild(i);
                var optionsButton = buttonParent.GetComponentInChildren<Button>();
                if (optionsButton != null)
                {
                    _optionButtons.Add(optionsButton);
                    Debug.Log(optionsButton.name);
                }
                else
                {
                    Debug.LogError("Button tidak ditemukan pada child: " + i);
                }
            }
        }

        private void ResetButtons()
        {
            foreach (var button in _optionButtons)
            {
                button.onClick.RemoveAllListeners();
            }
            _optionButtons.Clear();
        }

        // !-- Controller Fields
        private void HandleNavigation()
        {
            if (_playerInputHandler.IsPressNavigate(NavigateState.Up))
            {
                simpleScrollSnap.GoToNextPanel();
            }
            else if (_playerInputHandler.IsPressNavigate(NavigateState.Down))
            {
                simpleScrollSnap.GoToPreviousPanel();
            }
        }

        private void HandleInteraction()
        {
            if (_playerInputHandler.IsPressInteract())
            {
                ExecuteInteraction();
            }
        }

        private void ExecuteInteraction()
        {
            var selectIndex = simpleScrollSnap.SelectedPanel;
            if (_optionButtons[selectIndex].TryGetComponent(out InteractButton button))
            {
                Debug.Log($"interact button {button.InteractType}");
                _optionButtons[selectIndex].onClick.Invoke();
                if (button.InteractType != InteractType.Item) return;
                Debug.LogWarning("interact item object!");
                // _optionButtons.RemoveAt(selectIndex);
            }
        }

        // !-- Button Methods
        private void DialogueButton()
        {

        }
        
        private void ItemButton()
        {

        }
        
        private void ExitButton()
        {
            _interactArea.IsInteracting = false;
            interactOptionsUI.SetActive(false);
            ResetButtons();
        }

        #endregion

    }
}
