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

    [Header("Cutscene Status")]
    [SerializeField] protected bool cutsceneTriggered; // Bisa diubah di Inspector

    protected virtual void Start()
    {
        // Sinkronisasi nilai dari PlayerPrefs saat game dimulai
        cutsceneTriggered = PlayerPrefs.GetInt(cutsceneID, 0) == 1;

        if (cutsceneTriggered)
        {
            gameObject.SetActive(false); // Nonaktifkan jika sudah dimainkan
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

    protected virtual void OnDialogComplete()
    {
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

    protected virtual void Sequence01() { }
    protected virtual void Sequence02() { }
    protected virtual void Sequence03() { }

    protected void StartDialog(TextAsset inkJSON, Animator emoteAnimator)
    {
        isDialogActive = true;
        dialogueManager.EnterDialogueMode(inkJSON, emoteAnimator);
    }

    // Method untuk menandai cutscene sudah dijalankan
    protected void MarkCutsceneAsTriggered()
    {
        SetCutsceneStatus(true);
    }

    // Setter untuk mengatur status cutscene melalui Inspector & kode
    public void SetCutsceneStatus(bool isTriggered)
    {
        cutsceneTriggered = isTriggered;
        PlayerPrefs.SetInt(cutsceneID, isTriggered ? 1 : 0);
        PlayerPrefs.Save();
        gameObject.SetActive(!isTriggered); // Aktifkan/nonaktifkan cutscene berdasarkan status
    }

    // Getter untuk mendapatkan status cutscene
    public bool GetCutsceneStatus()
    {
        return cutsceneTriggered;
    }
}
