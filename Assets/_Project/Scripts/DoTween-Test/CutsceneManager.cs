using UnityEngine;
using DG.Tweening;
using ATBMI.Entities.Player;

public class CutsceneManager : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public DialogManager dialogManager;
    private Animator playerAnimator;
    private PlayerController playerController;

    void Start()
    {
        playerAnimator = player?.GetComponent<Animator>(); 
        playerController = player?.GetComponent<PlayerController>();

        if (playerAnimator == null)
        {
            Debug.LogError("❌ ERROR: Animator tidak ditemukan pada karakter pemain!");
        }
        if (playerController == null)
        {
            Debug.LogError("❌ ERROR: PlayerController tidak ditemukan pada karakter pemain!");
        }
        if (dialogManager == null)
        {
            Debug.LogError("❌ ERROR: DialogManager tidak diassign di Inspector!");
        }
    }

    public void PlayCutscene()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        Sequence cutscene = DOTween.Sequence();

        // 🔹 Pemain berjalan ke kanan
        cutscene.AppendCallback(() => SafeUpdateDirection(1));
        cutscene.AppendCallback(() => SafeSetAnimation("isWalking", true));
        cutscene.Append(player.DOMoveX(player.position.x + 5f, 3f).SetEase(Ease.Linear));
        cutscene.Join(cameraTransform.DOMoveX(cameraTransform.position.x + 5f, 3f).SetEase(Ease.InOutSine));
        cutscene.AppendCallback(() => SafeSetAnimation("isWalking", false));

        // 🔹 Dialog pertama
        cutscene.AppendCallback(() => SafeShowDialog("Aku merasa ada sesuatu yang aneh di depan...", "Dewa"));
        cutscene.AppendInterval(3f);
        cutscene.AppendCallback(() => SafeHideDialog());

        // 🔹 Pemain berjalan ke kiri
        cutscene.AppendCallback(() => SafeUpdateDirection(-1));
        cutscene.AppendCallback(() => SafeSetAnimation("isWalking", true));
        cutscene.Append(player.DOMoveX(player.position.x - 5f, 2f).SetEase(Ease.Linear));
        cutscene.Join(cameraTransform.DOMoveX(cameraTransform.position.x - 5f, 2f).SetEase(Ease.InOutSine));
        cutscene.AppendCallback(() => SafeSetAnimation("isWalking", false));

        // 🔹 Dialog kedua
        cutscene.AppendCallback(() => SafeShowDialog("Apa itu? Sepertinya ada seseorang di sana.", "Dewa"));
        cutscene.AppendInterval(3f);
        cutscene.AppendCallback(() => SafeHideDialog());

        // 🔹 Cutscene selesai
        cutscene.AppendInterval(2f);
        cutscene.AppendCallback(() => EndCutscene());
    }

    void SafeShowDialog(string message, string speakerName)
    {
        if (dialogManager != null)
        {
            dialogManager.dialogBox.SetActive(true);
            dialogManager.dialogText.text = message;
            dialogManager.dialogNameText.text = speakerName;
            dialogManager.ShowDialog(message, speakerName);
        }
        else
        {
            Debug.LogError("❌ ERROR: DialogManager tidak diassign di Inspector!");
        }
    }

    void SafeHideDialog()
    {
        if (dialogManager != null)
        {
            dialogManager.HideDialog();
        }
    }

    void SafeSetAnimation(string parameter, bool value)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool(parameter, value);
        }
        else
        {
            Debug.LogError("❌ ERROR: Animator tidak ditemukan pada karakter!");
        }
    }

    void SafeUpdateDirection(int direction)
    {
        if (player != null)
        {
            player.localScale = new Vector3(direction, 1, 1);
        }
        else
        {
            Debug.LogError("❌ ERROR: Transform pemain tidak ditemukan!");
        }
    }

    void EndCutscene()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        else
        {
            Debug.LogError("❌ ERROR: PlayerController tidak ditemukan!");
        }
    }
}
