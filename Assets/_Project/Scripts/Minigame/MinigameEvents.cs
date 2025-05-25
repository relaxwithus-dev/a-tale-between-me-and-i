using System;

namespace ATBMI.Minigame
{
    public static class MinigameEvents
    {
        public static event Action OnEnterMinigame;
        public static event Action OnActivateTrigger;
        public static event Action OnWinMinigame;
        public static event Action OnLoseMinigame;
        
        public static void EnterMinigameEvent() => OnEnterMinigame?.Invoke();
        public static void ActivateTriggerEvent() => OnActivateTrigger?.Invoke();
        public static void WinMinigameEvent() => OnWinMinigame?.Invoke();
        public static void LoseMinigameEvent() => OnLoseMinigame?.Invoke();
        
    }
}