using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DanielLochner.Assets.SimpleScrollSnap;

namespace ATBMI.Entities.Player
{
    public class InteractOptions : MonoBehaviour
    {
        #region Fields & Property

        [Header("UI")]
        [SerializeField] private GameObject interactOptionsUI;
        [SerializeField] private TextMeshProUGUI optionInfoTextUI;
        [SerializeField] private Sprite[] buttonSprites;

        private readonly Button[] _optionButtons;

        // Reference
        public SimpleScrollSnap simpleScrollSnap;
        private InteractArea _interactArea;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _interactArea = GetComponent<InteractArea>();
        }

        private void Start()
        {

        }

        #endregion

        #region Methods

        private void InitilizeButtons()
        {
            var optionsChildCount = interactOptionsUI.transform.childCount;
            for (var i = 0; i < optionsChildCount; i++)
            {
                var options = interactOptionsUI.transform.GetChild(i);
                if (!options.TryGetComponent(out Button button)) continue;
                _optionButtons[i] = button;
            }
        }
        
        // private void InitializeOptions(InteractionBase interaction)
        // {
        //     // Setup Sprite
        //     var optionCount = _optionButtons.Length;
        //     for (var i = 0; i < optionCount; i++)
        //     {
        //         var button = _optionButtons[i];
        //         var buttonSprite = buttonSprites[i];
        //         button.image.sprite = buttonSprite;
        //     }
        // }

        private void OpenInteractOptions()
        {
            interactOptionsUI.SetActive(true);
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

        }

        #endregion

    }
}
