using System;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public static class DialogueEvents
    {
        // Main Event
        public static event Action<TextAsset> OnEnterDialogue;
        public static event Action<TextAsset> OnEnterItemDialogue;
        public static event Action<string> PlayDialogueAnim;
        public static event Action StopDialogueAnim;
        public static event Action<string> PlayEmojiAnim;
        public static event Action<string, Transform> RegisterNPCTipTarget;
        public static event Action<string, Transform> RegisterNPCEmojiTarget;

        // Adjustment Event
        public static event Action<string> UpdateDialogueUIPos;
        public static event Action UpdateDialogueChoicesUIPos;
        public static event Action<string> UpdateDialogueEmojiPos;
        public static event Action<int> AdjustDialogueUISize;
        public static event Action<int> AdjustDialogueChoicesUISize;

        // Behaviour events
        public static event Action<bool> PlayerRun;

        // Main Event - Caller
        public static void EnterDialogueEvent(TextAsset defaultDialogue) => OnEnterDialogue?.Invoke(defaultDialogue);
        public static void EnterItemDialogueEvent(TextAsset itemDialogue) => OnEnterItemDialogue?.Invoke(itemDialogue);
        public static void PlayDialogueAnimEvent(string tag) => PlayDialogueAnim?.Invoke(tag);
        public static void StopDialogueAnimEvent() => StopDialogueAnim?.Invoke();
        public static void PlayEmojiAnimEvent(string tag) => PlayEmojiAnim?.Invoke(tag);
        public static void RegisterNPCTipTargetEvent(string npcName, Transform tipTarget) => RegisterNPCTipTarget?.Invoke(npcName, tipTarget);
        public static void RegisterNPCEmojiTargetEvent(string npcName, Transform emojiTarget) => RegisterNPCEmojiTarget?.Invoke(npcName, emojiTarget);

        // Adjustment Event - Caller
        public static void UpdateDialogueUIPosEvent(string tag) => UpdateDialogueUIPos?.Invoke(tag);
        public static void UpdateDialogueChoicesUIPosEvent() => UpdateDialogueChoicesUIPos?.Invoke();
        public static void UpdateDialogueEmojiPosEvent(string tag) => UpdateDialogueEmojiPos?.Invoke(tag);
        public static void AdjustDialogueUISizeEvent(int count) => AdjustDialogueUISize?.Invoke(count);
        public static void AdjustDialogueChoicesUISizeEvent(int count) => AdjustDialogueChoicesUISize?.Invoke(count);

        // behaviour events
        public static void PlayerRunEvent(bool isRunning) => PlayerRun?.Invoke(isRunning);
    }
}
