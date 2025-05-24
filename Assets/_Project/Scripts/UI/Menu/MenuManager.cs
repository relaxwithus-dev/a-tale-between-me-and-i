using ATBMI.Audio;
using ATBMI.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace ATBMI.UI.Menu
{
    public class MenuManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("UI")] 
        [SerializeField] private GameObject menuPanelUI;
        [SerializeField] private GameObject playPanelUI;
        [SerializeField] private GameObject optionPanelUI;

        [Space] 
        [SerializeField] private Button playButtonUI;
        [SerializeField] private Button optionButtonUI;
        [SerializeField] private Button exitButtonUI;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
            InitPanel();
            InitButton();
        }
        
        // Initialize
        private void InitPanel()
        {
            menuPanelUI.SetActive(true);
            playPanelUI.SetActive(false);
            optionPanelUI.SetActive(false);
            
            var navigation = SceneNavigation.Instance;
            if (navigation.IsInitiateComplete)
            {
                navigation.Fader.FadeIn();
            }
        }

        private void InitButton()
        {
            playButtonUI.onClick.AddListener(OnPlayButton);
            optionButtonUI.onClick.AddListener(OnOptionButton);
            exitButtonUI.onClick.AddListener(OnExitButton);
        }
        
        // Core
        private void OnPlayButton()
        {
            AudioManager.Instance.PlayAudio(Musics.SFX_Button);
            playPanelUI.SetActive(true);
        }

        private void OnOptionButton()
        {
            AudioManager.Instance.PlayAudio(Musics.SFX_Button);
            optionPanelUI.SetActive(true);
        }

        private void OnExitButton()
        {
            AudioManager.Instance.PlayAudio(Musics.SFX_Button);
            Application.Quit();
        }

        #endregion
    }
}