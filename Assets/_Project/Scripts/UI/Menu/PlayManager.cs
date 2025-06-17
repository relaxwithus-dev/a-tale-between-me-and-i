using UnityEngine;
using UnityEngine.UI;
using ATBMI.Audio;
using ATBMI.Scene;
using ATBMI.Gameplay.Controller;

namespace ATBMI.UI.Menu
{
    public class PlayManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Scene")]
        [SerializeField] private SceneAsset targetSceneAsset;
        
        [Header("UI")] 
        [SerializeField] private GameObject playPanelUI;
        [SerializeField] private Button[] saveButtonsUI;
        
        [Header("Reference")]
        [SerializeField] private MenuManager menuManager;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
            playPanelUI.SetActive(false);
            for (var i = 0; i < saveButtonsUI.Length; i++)
            {
                var button = saveButtonsUI[i];
                
                button.enabled = true;
                button.interactable = i == 0;
                if (button.interactable)
                    button.onClick.AddListener(() => OnPlayButton(button));
            }
        }
        
        private void Update()
        {
            if (!playPanelUI.activeSelf) return;
            if (GameInputHandler.Instance.IsTapClose)
            {
                playPanelUI.SetActive(false);
                menuManager.OpenMenuPanel();
            }
        }
        
        // Core
        private void OnPlayButton(Button btn)
        {
            btn.enabled = false;
            AudioEvent.FadeOutAudioEvent();
            AudioManager.Instance.PlayAudio(Musics.SFX_Button);
            SceneNavigation.Instance.SwitchSceneSection(isToMenu: false, targetSceneAsset);
        }
        
        #endregion
    }
}