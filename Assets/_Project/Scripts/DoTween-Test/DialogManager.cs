using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox; // Dialog Box UI
    public TextMeshProUGUI dialogText; // Text di dalam DialogBox
    public CanvasGroup dialogCanvasGroup; // Canvas Group untuk animasi fade
    private bool isDialogActive = false;

    void Start()
    {
        dialogBox.SetActive(false); // Pastikan DialogBox tersembunyi saat game dimulai
    }

    public void ShowDialog(string message)
{
    if (dialogBox == null || dialogText == null || dialogCanvasGroup == null)
    {
        Debug.LogError("ShowDialog() gagal: Pastikan semua reference sudah diassign!");
        return;
    }

    Debug.Log("Menampilkan DialogBox...");
    
    isDialogActive = true;
    dialogBox.SetActive(true); // Pastikan dialogBox aktif
    dialogText.text = message; // Masukkan teks ke dialog
    
    dialogCanvasGroup.alpha = 0;
    dialogCanvasGroup.DOFade(1, 0.5f);
}

    public void HideDialog()
    {
        isDialogActive = false;

        // Animasi fade-out, lalu sembunyikan dialog box
        dialogCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            dialogBox.SetActive(false);
        });
    }
}