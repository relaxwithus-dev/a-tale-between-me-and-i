using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ATBMI.Gameplay.Handler;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ATBMI.Minigame
{
    public class ArrowView : MinigameView
    {
        #region Structs

        [Serializable]
        private struct Arrows
        {
            public string name;
            public Sprite form;
        }
        
        [Serializable]
        private struct ArrowAttribute
        {
            public int count;
            public float duration;
        }

        #endregion
        
        #region Fields & Properties
        
        [Header("Attribute")] 
        [SerializeField] private ArrowAttribute[] arrowAttributes;
        [SerializeField] private Arrows[] arrowForms;
    
        private float _elapsedTime;
        private int _interactCount;
        private int _currentArrowIndex;
        
        private ArrowAttribute _currentAttribute;
        private readonly List<string> _spawnedArrowNames = new();
        
        [Header("UI")]
        [SerializeField] private Image[] arrowImages;
        [SerializeField] private Slider arrowSlider;
        
        // Reference
        private GameInputHandler _input;
        
        #endregion
        
        #region Methods
        
        // Core
        public override void EnterMinigame() 
        {
            base.EnterMinigame();
            
            // Initiate attribute
            _elapsedTime = 0f;
            _currentArrowIndex = 0;
            
            _spawnedArrowNames.Clear();
            _input ??= GameInputHandler.Instance;
            _currentAttribute = arrowAttributes[_interactCount];
            
            arrowSlider.value = MAX_SLIDER_VALUE;
            
            // Reset image
            ResetArrowImage(isNonActivate: true);
            
            // Initiate Image
            for (var i = 0; i < _currentAttribute.count; i++)
            {
                if (arrowImages.Length < _currentAttribute.count)
                {
                    Debug.LogWarning("Arrow image count is less than arrow count!");
                    break;
                }
                
                var arrow = arrowImages[i];
                var arrowForm = arrowForms[Random.Range(0, arrowForms.Length)];
                
                arrow.sprite = arrowForm.form;
                arrow.gameObject.SetActive(true);
                _spawnedArrowNames.Add(arrowForm.name);
            }
        }
        
        protected override void RunMinigame()
        {
            base.RunMinigame();
            
            HandleArrowTime();
            HandleArrowGameplay();
        }
        
        protected override void ExitMinigame()
        {
            base.ExitMinigame();
            ResetArrowImage(isNonActivate: true);
        }
        
        private void HandleArrowGameplay()
        {
            var inputName = GetArrowInputName();
            if (!string.IsNullOrEmpty(inputName))
            {
                if (_spawnedArrowNames[_currentArrowIndex] == inputName)
                {
                    arrowImages[_currentArrowIndex].color = Color.white;
                    _currentArrowIndex++;
                }
                else
                {
                    _currentArrowIndex = 0;
                    ResetArrowImage(isNonActivate: false);
                }
            }

            if (_currentArrowIndex >= _currentAttribute.count)
            {
                _interactCount = Mathf.Clamp(_interactCount + 1, 0, arrowAttributes.Length - 1);
                ExitMinigame();
            }
        }
        
        private void HandleArrowTime()
        {
            _elapsedTime += Time.deltaTime;
            arrowSlider.value = Mathf.Lerp(MAX_SLIDER_VALUE, MIN_SLIDER_VALUE, _elapsedTime / _currentAttribute.duration);
            
            if (arrowSlider.value <= MIN_SLIDER_VALUE)
            {
                arrowSlider.value = MIN_SLIDER_VALUE;
                ExitMinigame();
            }
        }
        
        private void ResetArrowImage(bool isNonActivate)
        {
            foreach (var arrow in arrowImages)
            {
                if (!arrow.gameObject.activeSelf)
                    continue;
                
                arrow.color = Color.black;
                if (isNonActivate)
                    arrow.gameObject.SetActive(false);
            }
        }
        
        private string GetArrowInputName()
        {
            if (_input.IsArrowUp) return "Arrow Up";
            if (_input.IsArrowDown) return "Arrow Down";
            if (_input.IsArrowLeft) return "Arrow Left";
            if (_input.IsArrowRight) return "Arrow Right";
            return null;
        }
        
        #endregion
    }
}