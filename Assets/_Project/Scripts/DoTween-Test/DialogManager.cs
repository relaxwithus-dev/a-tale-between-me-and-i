using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox; // UI DialogBox
    public TextMeshProUGUI dialogText; // Text dialog
    public CanvasGroup dialogCanvasGroup; // Untuk animasi fade-in/out

    private bool isDialogActive = false;

    void Start()
    {
        if (dialogBox == null || dialogText == null || dialogCanvasGroup == null)
        {
            Debug.LogError("DialogManager: Ada komponen yang belum diassign! Pastikan dialogBox, dialogText, dan dialogCanvasGroup sudah diatur di Inspector.");
            return;
        }

        dialogBox.SetActive(false); // Pastikan dialogBox tidak muncul saat game dimulai
    }

    public void ShowDialog(string message)
    {
        if (dialogBox == null || dialogText == null || dialogCanvasGroup == null)
        {
            Debug.LogError("ShowDialog() gagal: Pastikan semua reference sudah diassign!");
            return;
        }

        isDialogActive = true;
        dialogBox.SetActive(true); // Aktifkan dialogBox
        dialogText.text = ""; // Kosongkan teks sebelum animasi mengetik

        dialogCanvasGroup.alpha = 0;
        dialogCanvasGroup.DOFade(1, 0.5f).OnComplete(() =>
        {
            StartCoroutine(TypeDialog(message)); // Mulai efek mengetik
        });
    }

    public void HideDialog()
    {
        if (dialogCanvasGroup == null)
        {
            Debug.LogError("HideDialog() gagal: CanvasGroup tidak diassign!");
            return;
        }

        isDialogActive = false;
        dialogCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            dialogBox.SetActive(false); // Nonaktifkan setelah fade-out selesai
        });
    }

    IEnumerator TypeDialog(string message)
    {
        dialogText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.05f); // Efek mengetik
        }
    }

    public bool IsDialogActive()
    {
        return isDialogActive;
    }
}