using System;
using UnityEngine;

namespace ATBMI.Gameplay.Event
{
    public static class UIEvents
    {
        public static event Action OnSelectTabInventory;
        public static event Action OnDeselectTabInventory;
        public static event Action OnSelectTabQuest;
        public static event Action OnDeselectTabQuest;
        public static event Action OnSelectTabSetting;
        public static event Action OnDeselectTabSetting;

        public static void OnSelectTabInventoryEvent() => OnSelectTabInventory?.Invoke();
        public static void OnDeselectTabInventoryEvent() => OnDeselectTabInventory?.Invoke();
        public static void OnSelectTabQuestEvent() => OnSelectTabQuest?.Invoke();
        public static void OnDeselectTabQuestEvent() => OnDeselectTabQuest?.Invoke();
        public static void OnSelectTabSettingEvent() => OnSelectTabSetting?.Invoke();
        public static void OnDeselectTabSettingEvent() => OnDeselectTabSetting?.Invoke();
    }
}
