using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using ATBMI.Scene;
using ATBMI.Audio;
using ATBMI.UI.Ingame;
using ATBMI.Gameplay.Event;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Quest
{
    public class SettingHandler : MonoBehaviour
    {
        [System.Serializable]
        public class OptionUI
        {
            public InGameSettingEnum optionType;
            public GameObject selector;
            [ShowIf("@optionType != InGameSettingEnum.Exit")] public TextMeshProUGUI label;
            [ShowIf("@optionType != InGameSettingEnum.Exit")] public GameObject leftArrow;
            [ShowIf("@optionType != InGameSettingEnum.Exit")] public TextMeshProUGUI valueText;
            [ShowIf("@optionType != InGameSettingEnum.Exit")] public GameObject rightArrow;
        }
        
        [SerializeField] private UIMenu uiMenu;
        [SerializeField] private GameObject settingTab;

        [SerializeField] private OptionUI[] optionUI;

        private Dictionary<InGameSettingEnum, OptionUI> optionUIDict;
        private List<InGameSettingEnum> optionOrder;
        private int currentSelectionIndex;

        private int volume = 100;
        // TODO: change it to actual screen mode 
        private readonly string[] screenModes = { "Fullscreen", "Windowed", "Borderless" };
        private int screenIndex = 1;

        private void Awake()
        {
            // Build dictionary and ordered list
            optionUIDict = new Dictionary<InGameSettingEnum, OptionUI>();
            optionOrder = new List<InGameSettingEnum>();

            foreach (var opt in optionUI)
            {
                optionUIDict[opt.optionType] = opt;
                optionOrder.Add(opt.optionType);
            }

            ResetSelector();
        }

        private void OnEnable()
        {
            UIEvents.OnSelectTabSetting += SelectSetting;
        }

        private void OnDisable()
        {
            UIEvents.OnSelectTabSetting -= SelectSetting;
        }

        private void SelectSetting()
        {
            ResetSelector();
        }

        private void ResetSelector()
        {
            currentSelectionIndex = 0;

            for (int i = 0; i < optionUI.Length; i++)
            {
                if (i > 0)
                {
                    optionUI[i].selector.SetActive(false);
                    if (optionUI[i].optionType != InGameSettingEnum.Exit)
                    {
                        optionUI[i].leftArrow.SetActive(false);
                        optionUI[i].rightArrow.SetActive(false);
                    }
                }
                else
                {
                    optionUI[i].selector.SetActive(true);
                    if (optionUI[i].optionType != InGameSettingEnum.Exit)
                    {
                        optionUI[i].leftArrow.SetActive(true);
                        optionUI[i].rightArrow.SetActive(true);
                    }
                }
            }
        }

        private void Start()
        {
            StartCoroutine(DelayedInit());
        }

        private IEnumerator DelayedInit()
        {
            // Wait until AudioManager.Instance is not null add other manager if needed
            while (AudioManager.Instance == null)
                yield return null;

            UpdateAllOptions();
        }

        private void Update()
        {
            if (!settingTab.activeInHierarchy)
            {
                return;
            }

            if (GameInputHandler.Instance.IsArrowDown)
            {
                currentSelectionIndex = (currentSelectionIndex + 1) % optionOrder.Count;
                UpdateAllOptions();
            }
            else if (GameInputHandler.Instance.IsArrowUp)
            {
                currentSelectionIndex = (currentSelectionIndex - 1 + optionOrder.Count) % optionOrder.Count;
                UpdateAllOptions();
            }

            if (GameInputHandler.Instance.IsArrowLeft || GameInputHandler.Instance.IsArrowRight)
            {
                int delta = GameInputHandler.Instance.IsArrowRight ? 1 : -1;

                switch (optionOrder[currentSelectionIndex])
                {
                    case InGameSettingEnum.Volume:
                        volume = Mathf.Clamp(volume + delta * 10, 0, 100);
                        break;

                    case InGameSettingEnum.ScreenMode:
                        screenIndex = (screenIndex + delta + screenModes.Length) % screenModes.Length;
                        break;
                }

                UpdateAllOptions();
            }

            if ((GameInputHandler.Instance.IsTapSelect || GameInputHandler.Instance.IsTapSubmit) && optionOrder[currentSelectionIndex] == InGameSettingEnum.Exit)
            {
                uiMenu.CloseMenu();
                GameEvents.GameExitEvent();
                AudioEvent.FadeOutAudioEvent();
                AudioManager.Instance.PlayAudio(Musics.SFX_Button);
                SceneNavigation.Instance.SwitchSceneSection(isToMenu: true);
            }
        }

        private void UpdateAllOptions()
        {
            foreach (var kvp in optionUIDict)
            {
                var type = kvp.Key;
                var ui = kvp.Value;
                bool isSelected = type == optionOrder[currentSelectionIndex];

                ui.selector.SetActive(isSelected);
                if (ui.optionType != InGameSettingEnum.Exit)
                {
                    ui.leftArrow.SetActive(isSelected);
                    ui.rightArrow.SetActive(isSelected);
                }

                switch (type)
                {
                    case InGameSettingEnum.Volume:
                        ui.valueText.text = $"{volume}%";
                        // TODO: change if needed
                        AudioManager.Instance.SetMasterVolume(volume / 100f);
                        break;

                    case InGameSettingEnum.ScreenMode:
                        ui.valueText.text = screenModes[screenIndex];
                        // TODO: change the screen mode
                        break;
                }
            }
        }
    }
}
