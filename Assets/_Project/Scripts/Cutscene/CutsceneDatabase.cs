using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CutsceneData", menuName = "Data/Cutscene Data", order = 1)]
public class CutsceneDatabase : ScriptableObject
{
    [System.Serializable]
    public class CutsceneState
    {
        public string cutsceneID;
        public bool isTriggered;
    }

    public List<CutsceneState> cutsceneStates = new List<CutsceneState>();

    public bool GetCutsceneState(string cutsceneID)
    {
        var state = cutsceneStates.Find(x => x.cutsceneID == cutsceneID);
        if (state != null)
        {
            Debug.Log($"Cutscene {cutsceneID} state ditemukan: {state.isTriggered}");
            return state.isTriggered;
        }
        Debug.Log($"Cutscene {cutsceneID} state tidak ditemukan, mengembalikan false.");
        return false;
    }

    public void SetCutsceneState(string cutsceneID, bool isTriggered)
    {
        var state = cutsceneStates.Find(x => x.cutsceneID == cutsceneID);
        if (state != null)
        {
            state.isTriggered = isTriggered;
            Debug.Log($"Cutscene {cutsceneID} state diubah menjadi: {isTriggered}");
        }
        else
        {
            cutsceneStates.Add(new CutsceneState { cutsceneID = cutsceneID, isTriggered = isTriggered });
            Debug.Log($"Cutscene {cutsceneID} ditambahkan dengan state: {isTriggered}");
        }
    }

    public void ResetAllCutscenes()
    {
        foreach (var state in cutsceneStates)
        {
            state.isTriggered = false;
        }
        Debug.Log("Semua cutscene telah di-reset.");
    }
}