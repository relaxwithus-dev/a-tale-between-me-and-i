using UnityEngine;
using DG.Tweening;
using System.Collections;
using ATBMI.Entities.Player;

public class CutsceneManager : MonoBehaviour
{
    public Transform player;
    public Transform cameraTransform;
    public DialogManager dialogManager;
    public CanvasGroup blackScreen;
    public CutsceneData cutsceneData; // Cutscene yang akan dimainkan

    private Animator playerAnimator;
    private PlayerController playerController;

    void Start()
    {
        playerAnimator = player?.GetComponent<Animator>();
        playerController = player?.GetComponent<PlayerController>();

        if (cutsceneData == null)
        {
            Debug.LogError("❌ ERROR: Tidak ada cutscene yang diassign!");
            return;
        }
    }

    public void StartCutscene() // Metode agar bisa dipanggil dari luar
    {
        if (playerController != null)
        {
            playerController.enabled = false; // Nonaktifkan kontrol pemain
        }

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isWalking", false); // Pastikan animasi Idle
            playerAnimator.Play("Idle"); // Paksa animasi kembali ke Idle
        }

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        foreach (CutsceneEvent cutsceneEvent in cutsceneData.events)
        {
            switch (cutsceneEvent.type)
            {
                case CutsceneType.ShowDialog:
                    dialogManager.ShowDialog(cutsceneEvent.dialog, cutsceneEvent.speaker);
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); // Tunggu input
                    yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space)); // Pastikan tombol dilepas
                    dialogManager.HideDialog();
                    break;

                case CutsceneType.MovePlayer:
                    if (playerAnimator != null)
                    {
                        playerAnimator.SetBool("isWalking", true);
                    }
                    yield return player.DOMoveX(player.position.x + cutsceneEvent.moveDistance, cutsceneEvent.moveTime).SetEase(Ease.Linear).WaitForCompletion();
                    if (playerAnimator != null)
                    {
                        playerAnimator.SetBool("isWalking", false);
                        playerAnimator.Play("Idle");
                    }
                    break;

                case CutsceneType.MoveCamera:
                    yield return cameraTransform.DOMoveX(cameraTransform.position.x + cutsceneEvent.moveDistance, cutsceneEvent.moveTime).SetEase(Ease.InOutSine).WaitForCompletion();
                    break;

                case CutsceneType.FadeScreen:
                    yield return blackScreen.DOFade(cutsceneEvent.moveDistance, cutsceneEvent.moveTime).WaitForCompletion();
                    break;
            }
        }

        EndCutscene();
    }


    private void EndCutscene()
    {
        if (playerController != null)
        {
            playerController.enabled = true; // Aktifkan kontrol pemain kembali
        }
        
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isWalking", false);
            playerAnimator.Play("Idle"); // Pastikan karakter tetap Idle setelah cutscene
        }
    }
}
