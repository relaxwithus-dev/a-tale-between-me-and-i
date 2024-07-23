using System;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public static class DialogEventHandler
    {
        // Main Event
        public static event Action<TextAsset> EnterDialogue;
        public static event Action<string> PlayDialogueAnim;
        public static event Action StopDialogueAnim;

        // Adjustment Event
        public static event Action<string> UpdateDialogueUIPos;
        public static event Action<string> UpdateDialogueChoicesUIPos;
        public static event Action<int> AdjustDialogueUISize;
        public static event Action<int> AdjustDialogueChoicesUISize;

        // Main Event - Caller
        public static void EnterDialogueEvent(TextAsset inkJson) => EnterDialogue?.Invoke(inkJson);
        public static void PlayDialogueAnimEvent(string tag) => PlayDialogueAnim?.Invoke(tag);
        public static void StopDialogueAnimEvent() => StopDialogueAnim?.Invoke();
        
        // Adjustment Event - Caller
        public static void UpdateDialogueUIPosEvent(string tag) => UpdateDialogueUIPos?.Invoke(tag);
        public static void UpdateDialogueChoicesUIPosEvent(string tag) => UpdateDialogueChoicesUIPos?.Invoke(tag);
        public static void AdjustDialogueUISizeEvent(int count) => AdjustDialogueUISize?.Invoke(count);
        public static void AdjustDialogueChoicesUISizeEvent(int count) => AdjustDialogueChoicesUISize?.Invoke(count);
    }
}
