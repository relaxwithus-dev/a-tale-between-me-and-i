using UnityEngine;
using UnityEngine.UI;
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
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void Start()
        {
            saveButtonUI.onClick.AddListener(() => Debug.Log("Go!"));
        }
        
        private void Update()
        {
            if (!playPanelUI.activeSelf) return;
            if (GameInputHandler.Instance.IsTapBack)
            {
                playPanelUI.SetActive(false);
            }
        }

        #endregion
    }
}