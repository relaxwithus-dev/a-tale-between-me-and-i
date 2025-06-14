using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ATBMI.Audio;
using ATBMI.Gameplay.Handler;

namespace ATBMI.UI.Menu
{
    public class OptionManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("UI")]
        [SerializeField] private GameObject optionPanelUI;
        [SerializeField] private Image[] headerMenuUI;
        [SerializeField] private GameObject[] contentMenuUI;

        private int _selectedMenuIndex;
        private readonly List<TextMeshProUGUI> _headerTextUI = new();
        
        [Header("Color")]
        [SerializeField] private Color highlightBarColor;
        [SerializeField] private Color normalBarColor;
        [SerializeField] private Color highlightTextColor;
        [SerializeField] private Color normalTextColor;

        [Header("Reference")]
        [SerializeField] private MenuManager menuManager;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
            // Validate
            if (headerMenuUI.Length != contentMenuUI.Length)
            {
                Debug.LogWarning("header and content isn't equal!");
                return;
            }
            
            for (var i = 0; i < headerMenuUI.Length; i++)
            {
                var header = headerMenuUI[i];
                var headerText = header.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                if (i == 0)
                {
                    header.color = highlightBarColor;
                    headerText.color = highlightTextColor;
                    contentMenuUI[i].SetActive(true);
                }
                else
                {
                    header.color = normalBarColor;
                    headerText.color = normalTextColor;
                    contentMenuUI[i].SetActive(false);
                }
                
                _headerTextUI.Add(headerText);
            }
        }
        
        private void Update()
        {
            if (!optionPanelUI.activeSelf) return;

            if (GameInputHandler.Instance.IsTabRight)
                MoveToNext();
            else if (GameInputHandler.Instance.IsTabLeft)
                MoveToPrevious();
            else if (GameInputHandler.Instance.IsTapBack)
                CloseOptionPanel();
        }
        
        // Core
        private void MoveToNext()
        {
            AudioManager.Instance.PlayAudio(Musics.SFX_ChangeUI);
            if (_selectedMenuIndex >= headerMenuUI.Length - 1) return;
            
            _selectedMenuIndex++;
            HighlightMenu(_selectedMenuIndex);
            UnhighlightMenu(_selectedMenuIndex - 1);
        }
        
        private void MoveToPrevious()
        {
            AudioManager.Instance.PlayAudio(Musics.SFX_ChangeUI);
            if (_selectedMenuIndex <= 0) return;
            
            _selectedMenuIndex--;
            HighlightMenu(_selectedMenuIndex);
            UnhighlightMenu(_selectedMenuIndex + 1);
        }
        
        private void CloseOptionPanel()
        {
            optionPanelUI.SetActive(false);
            menuManager.OpenMenuPanel();
        }
        
        private void HighlightMenu(int index)
        {
            headerMenuUI[index].color = highlightBarColor;
            _headerTextUI[index].color = highlightTextColor;
            
            contentMenuUI[index].SetActive(true);
        }
        
        private void UnhighlightMenu(int index)
        {
            headerMenuUI[index].color = normalBarColor;
            _headerTextUI[index].color = normalTextColor;
            
            contentMenuUI[index].SetActive(false);
        }
        
        #endregion
    }
}