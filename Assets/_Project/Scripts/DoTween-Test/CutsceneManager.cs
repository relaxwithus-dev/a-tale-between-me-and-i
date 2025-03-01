using UnityEngine;
using DG.Tweening;
using ATBMI.Entities.Player;

public class CutsceneManager : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public DialogManager dialogManager; // Pastikan ini sudah dihubungkan di Inspector!
    private Animator PlayerAnimation;

    void Start()
    {
        PlayerAnimation = player.GetComponent<Animator>(); // Ambil Animator dari karakter

        if (PlayerAnimation == null)
        {
            Debug.LogError("Animator tidak ditemukan pada karakter pemain!");
        }
    }

    public void PlayCutscene()
    {
        // Nonaktifkan kontrol pemain
        player.GetComponent<PlayerController>().enabled = false;

        Sequence cutscene = DOTween.Sequence();

        // 🔹 Ubah arah sebelum bergerak ke kanan
        cutscene.AppendCallback(() => UpdateDirection(player.position.x + 5f));

        // Pemain berjalan ke kanan dan kamera mengikuti
        cutscene.AppendCallback(() => PlayerAnimation.SetBool("isWalking", true));
        cutscene.Append(player.DOMoveX(player.position.x + 5f, 3f).SetEase(Ease.Linear));
        cutscene.Join(cameraTransform.DOMoveX(cameraTransform.position.x + 5f, 3f).SetEase(Ease.InOutSine));

        // 🔹 Tambahkan dialog pertama
        cutscene.AppendCallback(() => PlayerAnimation.SetBool("isWalking", false));
        cutscene.AppendCallback(() => {
            Debug.Log("Memulai dialog pertama...");
            ShowDialog("Aku merasa ada sesuatu yang aneh di depan...");
        });
        cutscene.AppendInterval(3f);
        cutscene.AppendCallback(() => HideDialog());

        // 🔹 Ubah arah sebelum bergerak ke kiri
        cutscene.AppendCallback(() => UpdateDirection(player.position.x - 5f));

        // Pemain berjalan ke kiri
        cutscene.AppendCallback(() => PlayerAnimation.SetBool("isWalking", true));
        cutscene.AppendInterval(1f);
        cutscene.Append(player.DOMoveX(player.position.x - 5f, 2f).SetEase(Ease.Linear));
        cutscene.Join(cameraTransform.DOMoveX(cameraTransform.position.x - 5f, 2f).SetEase(Ease.InOutSine));
        cutscene.AppendCallback(() => PlayerAnimation.SetBool("isWalking", false));

        // 🔹 Tambahkan dialog kedua
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
        dialogManager.dialogBox.SetActive(true);
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

    // 🔹 **Fungsi untuk mengubah arah karakter**
    void UpdateDirection(float targetX)
    {
        if (targetX > player.position.x)
        {
            player.localScale = new Vector3(1, 1, 1); // Menghadap kanan
        }
        else
        {
            player.localScale = new Vector3(-1, 1, 1); // Menghadap kiri
        }
    }
}