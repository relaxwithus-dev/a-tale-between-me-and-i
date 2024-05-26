using System;
using UnityEngine;

public static class EventHandler
{
    public static event Action TickAction;
    public static void CallTickActionEvent()
    {
        if (TickAction != null)
            TickAction();
    }
    
    public static event Action<string> UpdateDialogueUIPos;
    public static void CallUpdateDialogueUIPosEvent(string tagValue)
    {
        if (UpdateDialogueUIPos != null)
            UpdateDialogueUIPos(tagValue);
    }
    
    public static event Action<string> UpdateDialogueChoicesUIPos;
    public static void CallUpdateDialogueChoicesUIPosEvent(string tagValue)
    {
        if (UpdateDialogueChoicesUIPos != null)
            UpdateDialogueChoicesUIPos(tagValue);
    }
    
    public static event Action<int> AdjustDialogueUISize;
    public static void CallAdjustDialogueUISizeEvent(int charCounter)
    {
        if (AdjustDialogueUISize != null)
            AdjustDialogueUISize(charCounter);
    }
    
    public static event Action<int> AdjustDialogueChoicesUISize;
    public static void CallAdjustDialogueChoicesUISizeEvent(int charCounter)
    {
        if (AdjustDialogueChoicesUISize != null)
            AdjustDialogueChoicesUISize(charCounter);
    }
    
    public static event Action<string> PlayDialogueAnim;
    public static void CallPlayDialogueAnimEvent(string tagValue)
    {
        if (PlayDialogueAnim != null)
            PlayDialogueAnim(tagValue);
    }
    
    public static event Action StopDialogueAnim;
    public static void CallStopDialogueAnimEvent()
    {
        if (StopDialogueAnim != null)
            StopDialogueAnim();
    }
    
    public static event Action<TextAsset> EnterDialogue;
    public static void CallEnterDialogueEvent(TextAsset INKJson)
    {
        if (EnterDialogue != null)
            EnterDialogue(INKJson);
    }
}
