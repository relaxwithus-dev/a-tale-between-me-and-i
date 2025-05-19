using UnityEngine;
using UnityEngine.UI;
using ATBMI.Audio;
using ATBMI.Scene;
using ATBMI.Gameplay.Handler;

namespace ATBMI.UI.Menu
{
    public class PlayManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")] 
        [SerializeField] private GameObject playPanelUI;
        [SerializeField] private Button saveButtonUI;
        
        [Header("Scene")]
        [SerializeField] private SceneAsset prologueAsset;
        
        [Header("Reference")]
        [SerializeField] private AudioController audioController;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
            saveButtonUI.onClick.AddListener(OnPlayButton);
        }
        
        private void Update()
        {
            if (!playPanelUI.activeSelf) return;
            if (GameInputHandler.Instance.IsTapBack)
            {
                playPanelUI.SetActive(false);
            }
        }
        
        // Core
        private void OnPlayButton()
        {
            audioController.FadeAudio(isFadeIn: false);
            AudioManager.Instance.PlayAudio(Musics.SFX_Button);
            SceneNavigation.Instance.SwitchSceneSection(isToMenu: false, prologueAsset);
        }

        #endregion
    }
}