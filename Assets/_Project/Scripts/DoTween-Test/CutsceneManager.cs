using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(FadeScreen(1, 2f)); // Fade ke hitam dalam 2 detik
        StartCoroutine(FadeScreen(0, 2f)); // Fade ke transparan dalam 2 detik


        playerAnimator = player?.GetComponent<Animator>();
        playerController = player?.GetComponent<PlayerController>();

        if (cutsceneData == null)
        {
            Debug.LogError("❌ ERROR: Tidak ada cutscene yang diassign!");
            return;
        }

        if (blackScreen == null)
        {
            Debug.LogError("❌ ERROR: BlackScreen (CanvasGroup) belum diassign!");
        }
        else
        {
            blackScreen.alpha = 0; // Pastikan layar transparan di awal
        }
    }

    public void StartCutscene() // Metode baru agar bisa dipanggil dari luar
    {
        StartCoroutine(PlayCutscene());
    }

    public IEnumerator PlayCutscene()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        foreach (CutsceneEvent cutsceneEvent in cutsceneData.events)
        {
            switch (cutsceneEvent.type)
            {
                case CutsceneType.ShowDialog:
                    dialogManager.ShowDialog(cutsceneEvent.dialog, cutsceneEvent.speaker);
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); // Pencet tombol untuk lanjut
                    dialogManager.HideDialog();
                    break;

                case CutsceneType.MovePlayer:
                    if (playerAnimator != null)
                    {
                        playerAnimator.SetBool("isWalking", true);
                    }
                    yield return player.DOMoveX(player.position.x + cutsceneEvent.moveDistance, cutsceneEvent.moveTime)
                        .SetEase(Ease.Linear)
                        .WaitForCompletion();
                    if (playerAnimator != null)
                    {
                        playerAnimator.SetBool("isWalking", false);
                    }
                    break;

                case CutsceneType.MoveCamera:
                    yield return cameraTransform.DOMoveX(cameraTransform.position.x + cutsceneEvent.moveDistance, cutsceneEvent.moveTime)
                        .SetEase(Ease.InOutSine)
                        .WaitForCompletion();
                    break;

                case CutsceneType.FadeScreen:
                    yield return StartCoroutine(FadeScreen(cutsceneEvent.moveDistance, cutsceneEvent.moveTime));
                    break;
            }
        }

        EndCutscene();
    }

    void EndCutscene()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    private IEnumerator FadeScreen(float targetAlpha, float duration)
    {
        if (blackScreen == null)
        {
            Debug.LogError("❌ ERROR: BlackScreen (CanvasGroup) tidak ditemukan!");
            yield break;
        }

        float startAlpha = blackScreen.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            blackScreen.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }

        blackScreen.alpha = targetAlpha;
    }
}
