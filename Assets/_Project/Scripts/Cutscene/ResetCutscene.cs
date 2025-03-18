using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CutsceneDatabase cutsceneDatabase;

    // public void ResetAllCutscenes()
    // {
    //     cutsceneDatabase.ResetAllCutscenes();
    //     Debug.Log("Semua cutscene telah di-reset!");
    // }

    // public void ResetSpecificCutscene(string cutsceneID)
    // {
    //     cutsceneDatabase.SetCutsceneState(cutsceneID, false);
    //     Debug.Log($"Cutscene {cutsceneID} telah di-reset!");
    // }
}