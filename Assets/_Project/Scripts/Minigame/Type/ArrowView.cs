using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ATBMI.Gameplay.Handler;

using Random = UnityEngine.Random;

namespace ATBMI.Minigame
{
    public class ArrowView : MinigameView
    {
        #region Fields & Properties

        [Serializable]
        private struct Arrows
        {
            public string name;
            public Sprite form;
        }
        
        [Header("Attribute")] 
        [SerializeField] private float arrowDuration;
        [SerializeField] private int arrowCount;
        [SerializeField] private Arrows[] arrowForms;
        
        private int _currentArrowIndex;
        private readonly List<string> _spawnedArrowNames = new();
        
        [Header("UI")]
        [SerializeField] private Image[] arrowImages;
        [SerializeField] private Slider arrowSlider;
        
        // Reference
        private GameInputHandler _input;
        
        #endregion
        
        #region Methods
        
        // Initialize
        protected override void InitOnStart()
        {
            base.InitOnStart();
            
            _currentArrowIndex = 0;
            _input = GameInputHandler.Instance;
            
            ResetArrowImage(isNonActivate: true);
            
            // Drop this when complete!
            EnterMinigame();
        }

        // Core
        public override void EnterMinigame() 
        {
            base.EnterMinigame();
            
            _currentArrowIndex = 0;
            if (_spawnedArrowNames.Count > 0)
                _spawnedArrowNames.Clear();
            
            for (var i = 0; i < arrowCount; i++)
            {
                if (arrowImages.Length < arrowCount)
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
            
            if (_currentArrowIndex >= arrowCount)
                ExitMinigame();
        }
        
        public override void ExitMinigame()
        {
            base.ExitMinigame();
            ResetArrowImage(isNonActivate: true);
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