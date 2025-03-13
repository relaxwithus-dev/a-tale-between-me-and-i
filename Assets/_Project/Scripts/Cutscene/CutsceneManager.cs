using UnityEngine;
using DG.Tweening;
using System.Collections;
using ATBMI.Dialogue;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager; // Assign via Inspector
    protected bool isDialogActive = false;
    protected int currentStep = 1;

    protected virtual void Start()
    {
        // Inisialisasi tambahan jika diperlukan
    }

    protected virtual void Update()
    {
        // Cek jika dialog selesai
        if (isDialogActive && !dialogueManager.IsDialoguePlaying)
        {
            isDialogActive = false;
            OnDialogComplete();
        }
    }

    protected virtual void OnDialogComplete()
    {
        // Panggil langkah berikutnya setelah dialog selesai
        NextStep(currentStep + 1);
    }

    protected virtual void NextStep(int step)
    {
        currentStep = step;
        switch (step)
        {
            case 1:
                Sequence01();
                break;
            case 2:
                Sequence02();
                break;
            case 3:
                Sequence03();
                break;
            default:
                Debug.Log("Cutscene selesai atau langkah tidak valid.");
                break;
        }
    }

    // Override method ini di child class
    protected virtual void Sequence01() { }
    protected virtual void Sequence02() { }
    protected virtual void Sequence03() { }

    protected void StartDialog(TextAsset inkJSON, Animator emoteAnimator)
    {
        isDialogActive = true;
        // Panggil method EnterDialogueMode dari DialogueManager
        dialogueManager.EnterDialogueMode(inkJSON, emoteAnimator);
    }
}