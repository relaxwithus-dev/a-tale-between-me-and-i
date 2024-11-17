using System;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public static class DialogEvents
    {
        // Main Event
        public static event Action EnterDialogue;
        public static event Action<string> PlayDialogueAnim;
        public static event Action StopDialogueAnim;

        // Adjustment Event
        public static event Action<string> UpdateDialogueUIPos;
        public static event Action<string> UpdateDialogueChoicesUIPos;
        public static event Action<int> AdjustDialogueUISize;
        public static event Action<int> AdjustDialogueChoicesUISize;

        // Main Event - Caller
        public static void EnterDialogueEvent() => EnterDialogue?.Invoke();
        public static void PlayDialogueAnimEvent(string tag) => PlayDialogueAnim?.Invoke(tag);
        public static void StopDialogueAnimEvent() => StopDialogueAnim?.Invoke();
        
        // Adjustment Event - Caller
        public static void UpdateDialogueUIPosEvent(string tag) => UpdateDialogueUIPos?.Invoke(tag);
        public static void UpdateDialogueChoicesUIPosEvent(string tag) => UpdateDialogueChoicesUIPos?.Invoke(tag);
        public static void AdjustDialogueUISizeEvent(int count) => AdjustDialogueUISize?.Invoke(count);
        public static void AdjustDialogueChoicesUISizeEvent(int count) => AdjustDialogueChoicesUISize?.Invoke(count);
    }
}
