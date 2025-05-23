using System;

namespace ATBMI.Gameplay.Event
{
    public static class GameEvents
    {
        // Events
        public static event Action OnGameStart;
        public static event Action OnGameExit;
        public static event Action OnChangeScene;
        
        // Caller
        public static void GameStartEvent() => OnGameStart?.Invoke();
        public static void GameExitEvent() => OnGameExit?.Invoke();
        public static void OnChangeSceneEvent() => OnChangeScene?.Invoke();
    }
}