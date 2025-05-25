using System.Collections;
using UnityEngine;
using ATBMI.Dialogue;
using ATBMI.Cutscene;
using ATBMI.Gameplay.Event;

namespace ATBMI.Minigame
{
    public class MinigameProgression : MonoBehaviour
    {
        [Header("Attribute")]
        [SerializeField] private TextAsset[] textAssets;
        [SerializeField] private CutsceneHandler cutsceneHandler;

        private bool _isWinning;
        
        // Unity Callbacks
        private void OnEnable()
        {
            // Minigame
            MinigameEvents.OnWinMinigame += WinMinigame;
            MinigameEvents.OnLoseMinigame += LoseMinigame;

            // Dialogue
            DialogueEvents.OnExitDialogue += EnterProgression;
        }
        
        private void OnDisable()
        {
            // Minigame
            MinigameEvents.OnWinMinigame -= WinMinigame;
            MinigameEvents.OnLoseMinigame -= LoseMinigame;

            // Dialogue
            DialogueEvents.OnExitDialogue -= EnterProgression;
        }

        private void Start()
        {
            _isWinning = false;
            cutsceneHandler.gameObject.SetActive(false);
            cutsceneHandler.HasConditionToPlay = true;
        }
        
        // Core
        private void WinMinigame()
        {
            cutsceneHandler.gameObject.SetActive(true);
            cutsceneHandler.HasConditionToPlay = false;
            DialogueManager.Instance.EnterDialogueMode(textAssets[0]);
            _isWinning = true;
        }
        private void LoseMinigame() => DialogueManager.Instance.EnterDialogueMode(textAssets[1]);
        
        private void EnterProgression()
        {
            if (!_isWinning) return;
            StartCoroutine(EnterProgressionRoutine());
        }

        private IEnumerator EnterProgressionRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            
            cutsceneHandler.gameObject.SetActive(true);
            cutsceneHandler.HasConditionToPlay = false;
            CutsceneManager.ModifyCutsceneEvent();
            cutsceneHandler.EnterDirectCutscene();
        }
    }
}