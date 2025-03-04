using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox; // UI Dialog Box
    public TextMeshProUGUI dialogText; // Teks dialog
    public TextMeshProUGUI dialogNameText; // Nama pembicara
    public CanvasGroup dialogCanvasGroup; // Untuk efek fade-in/out
    private bool isDialogActive = false;

    void Start()
    {
        dialogBox.SetActive(false); // Pastikan dialog tidak aktif saat game dimulai
    }

    public void ShowDialog(string message, string speakerName)
    {
        isDialogActive = true;
        dialogBox.SetActive(true); // Aktifkan dialog box
        dialogNameText.text = speakerName; // Set nama pembicara
        dialogText.text = message; // Set teks dialog

        // Animasi fade-in
        dialogCanvasGroup.alpha = 0;
        dialogCanvasGroup.DOFade(1, 0.5f);
    }

    public void HideDialog()
    {
        isDialogActive = false;
        dialogCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            dialogBox.SetActive(false); // Nonaktifkan setelah fade-out selesai
        });
    }

    public bool IsDialogActive()
    {
        return isDialogActive;
    }
}
