using System;

namespace ATBMI.Gameplay.Event
{
    public static class GameEvents
    {
        // Events
        public static event Action OnGameStart;
        public static event Action OnGamePause;
        public static event Action OnChangeScene;
        
        // Caller
        public static void GameStartEvent() => OnGameStart?.Invoke();
        public static void GamePauseEvent() => OnGamePause?.Invoke();
        public static void OnChangeSceneEvent() => OnChangeScene?.Invoke();
    }
}