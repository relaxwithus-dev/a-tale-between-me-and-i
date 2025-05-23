using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        private int _currentArrowIndex;
        private ArrowAttribute _attribute;
        
        private readonly List<string> _spawnedArrowNames = new();
        
        [Header("UI")]
        [SerializeField] private Image[] arrowImages;
        [SerializeField] private Slider timeSliderUI;
        
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
            _attribute = arrowAttributes[playingCount];
            
            timeSliderUI.value = MAX_SLIDER_VALUE;
            
            // Reset image
            ResetArrowImage(isNonActivate: true);
            
            // Initiate Image
            for (var i = 0; i < _attribute.count; i++)
            {
                if (arrowImages.Length < _attribute.count)
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
            
            // Win
            if (_currentArrowIndex >= _attribute.count)
            {
                playingCount = Mathf.Clamp(playingCount + 1, 0, arrowAttributes.Length - 1);
                ExitMinigame();
            }
        }
        
        private void HandleArrowTime()
        {
            _elapsedTime += Time.deltaTime;
            timeSliderUI.value = Mathf.Lerp(MAX_SLIDER_VALUE, MIN_SLIDER_VALUE, _elapsedTime / _attribute.duration);
            
            // Lose
            if (timeSliderUI.value <= MIN_SLIDER_VALUE)
            {
                timeSliderUI.value = MIN_SLIDER_VALUE;
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
            if (inputHandler.IsArrowUp) return "Arrow Up";
            if (inputHandler.IsArrowDown) return "Arrow Down";
            if (inputHandler.IsArrowLeft) return "Arrow Left";
            if (inputHandler.IsArrowRight) return "Arrow Right";
            return null;
        }
        
        #endregion
    }
}