using UnityEngine;
using DG.Tweening;
using ATBMI.Entities.Player;

public class CutsceneManager : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public DialogManager dialogManager; // Pastikan ini sudah dihubungkan di Inspector!

    public void PlayCutscene()
    {
        // Nonaktifkan kontrol pemain
        player.GetComponent<PlayerController>().enabled = false;

        Sequence cutscene = DOTween.Sequence();

        // Pemain berjalan maju dan kamera mengikuti
        cutscene.Append(player.DOMoveX(player.position.x + 5f, 3f).SetEase(Ease.Linear)); 
        cutscene.Join(cameraTransform.DOMoveX(cameraTransform.position.x + 5f, 3f).SetEase(Ease.InOutSine));

        // **🔹 Perbaikan: Tambahkan Debug Log untuk melihat apakah dialog dipanggil**
        cutscene.AppendCallback(() => {
            Debug.Log("Memulai dialog pertama...");
            ShowDialog("Aku merasa ada sesuatu yang aneh di depan...");
        });
        cutscene.AppendInterval(3f);
        cutscene.AppendCallback(() => HideDialog());

        // Tambahkan dialog kedua setelah berjalan sedikit lagi
        cutscene.AppendInterval(1f);
        cutscene.Append(player.DOMoveX(player.position.x + 8f, 2f).SetEase(Ease.Linear));
        cutscene.AppendCallback(() => {
            Debug.Log("Memulai dialog kedua...");
            ShowDialog("Apa itu? Sepertinya ada seseorang di sana.");
        });
        cutscene.AppendInterval(3f);
        cutscene.AppendCallback(() => HideDialog());

        // Tunggu sebentar sebelum cutscene selesai
        cutscene.AppendInterval(2f);
        cutscene.AppendCallback(() => EndCutscene());
    }

    void ShowDialog(string message)
    {
        dialogManager.dialogBox.SetActive(true); // Pastikan dialog box aktif
        dialogManager.dialogText.text = message;
        dialogManager.ShowDialog(message);
    }

    void HideDialog()
    {
        dialogManager.HideDialog();
    }

    void EndCutscene()
    {
        // Aktifkan kembali kontrol pemain
        player.GetComponent<PlayerController>().enabled = true;
    }
}