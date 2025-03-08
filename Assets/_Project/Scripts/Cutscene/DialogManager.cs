using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox; // UI Dialog Box
    public TextMeshProUGUI dialogText; // Teks dialog
    public TextMeshProUGUI dialogNameText; // Nama pembicara
    public CanvasGroup dialogCanvasGroup; // Untuk efek fade-in/out
    public float typingSpeed = 0.03f; // Kecepatan efek mesin tik (lebih cepat dari normal)

    private bool isDialogActive = false;
    private Coroutine typingCoroutine;
    private bool isTyping = false; // Untuk mendeteksi apakah sedang mengetik

    void Start()
    {
        dialogBox.SetActive(false);
        dialogCanvasGroup.alpha = 0;
    }

    public void ShowDialog(string message, string speakerName)
    {
        isDialogActive = true;
        dialogBox.SetActive(true);
        dialogNameText.text = speakerName;
        dialogText.text = "";

        dialogCanvasGroup.DOKill();
        dialogCanvasGroup.alpha = 1;
        dialogCanvasGroup.DOFade(1, 0.5f);

        // Mulai efek mesin tik
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void HideDialog()
    {
        if (isTyping) // Jika sedang mengetik, langsung tampilkan seluruh teks
        {
            StopCoroutine(typingCoroutine);
            dialogText.text = dialogText.text; // Pastikan teks lengkap tampil
            isTyping = false;
            return;
        }

        isDialogActive = false;
        dialogCanvasGroup.DOKill();
        dialogCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
        {
            dialogBox.SetActive(false);
        });
    }

    public bool IsDialogActive()
    {
        return isDialogActive;
    }
}
