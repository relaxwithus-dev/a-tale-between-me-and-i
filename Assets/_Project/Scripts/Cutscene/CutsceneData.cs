using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewCutscene", menuName = "Cutscene System/Cutscene Data")]
public class CutsceneData : ScriptableObject
{
    public List<CutsceneEvent> events;
}

[System.Serializable]
public class CutsceneEvent
{
    public CutsceneType type;
    public string speaker;
    public string dialog;
    public float moveDistance;
    public float moveTime;
}

public enum CutsceneType
{
    ShowDialog,
    MovePlayer,
    MoveCamera,
    FadeScreen
}
