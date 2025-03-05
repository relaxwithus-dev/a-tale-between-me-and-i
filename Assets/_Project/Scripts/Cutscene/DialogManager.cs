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
        dialogCanvasGroup.alpha = 0; // Pastikan alpha awal 0
    }

    public void ShowDialog(string message, string speakerName)
    {
        isDialogActive = true;
        dialogBox.SetActive(true); // Aktifkan dialog box
        dialogNameText.text = speakerName; // Set nama pembicara
        dialogText.text = message; // Set teks dialog

        // Pastikan dialog langsung terlihat dengan alpha 1
        dialogCanvasGroup.DOKill(); // Hentikan animasi sebelumnya
        dialogCanvasGroup.alpha = 1;
        dialogCanvasGroup.DOFade(1, 0.5f); // Animasi fade-in
    }

    public void HideDialog()
    {
        isDialogActive = false;
        dialogCanvasGroup.DOKill(); // Hentikan animasi sebelumnya
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
