using UnityEngine;
using DG.Tweening;
using System.Collections;
using ATBMI.Dialogue;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    protected bool isDialogActive = false;
    protected int currentStep = 1;

    [SerializeField] private string cutsceneID;

    [Header("Cutscene Database")]
    [SerializeField] private CutsceneDatabase cutsceneDatabase;

    [Header("Cutscene Status")]
    [SerializeField] protected bool cutsceneTriggered;

    protected virtual void Start()
    {
        cutsceneTriggered = cutsceneDatabase.GetCutsceneState(cutsceneID);

        if (cutsceneTriggered)
        {
            gameObject.SetActive(false);
            Debug.Log($"Cutscene {cutsceneID} sudah di-trigger sebelumnya, dinonaktifkan.");
        }
    }

    protected virtual void Update()
    {
        if (isDialogActive && !dialogueManager.IsDialoguePlaying)
        {
            isDialogActive = false;
            OnDialogComplete();
        }
    }

    protected virtual void OnDialogComplete() => NextStep(currentStep + 1);

    protected virtual void NextStep(int step)
    {
        currentStep = step;
        switch (step)
        {
            case 1: Sequence01(); break;
            case 2: Sequence02(); break;
            case 3: Sequence03(); break;
            default: EndCutscene(); break;
        }
    }

    protected virtual void Sequence01() { }
    protected virtual void Sequence02() { }
    protected virtual void Sequence03() { }

    protected void StartDialog(TextAsset inkJSON, Animator emoteAnimator)
    {
        isDialogActive = true;
        dialogueManager.EnterDialogueMode(inkJSON, emoteAnimator);
    }

    protected void MarkCutsceneAsTriggered()
    {
        if (!cutsceneTriggered)
        {
            cutsceneDatabase.SetCutsceneState(cutsceneID, true);
            cutsceneTriggered = true;
            gameObject.SetActive(false);
            Debug.Log($"Cutscene {cutsceneID} telah di-trigger dan dinonaktifkan.");
        }
    }

    public void ResetCutsceneState()
    {
        cutsceneDatabase.SetCutsceneState(cutsceneID, false);
        cutsceneTriggered = false;
        gameObject.SetActive(true);
        currentStep = 1;
        Debug.Log($"Cutscene {cutsceneID} telah di-reset.");
    }

    protected virtual void EndCutscene()
    {
        Debug.Log($"Cutscene {cutsceneID} selesai!");
        MarkCutsceneAsTriggered();
    }
}