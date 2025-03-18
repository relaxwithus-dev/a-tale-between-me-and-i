using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class TestCutscene : CutsceneManager
    {
        [SerializeField] private GameObject dewa;
        // [SerializeField] private GameObject ratna;
        // [SerializeField] private GameObject waffa;
        // [SerializeField] private GameObject luna;
        // [SerializeField] private GameObject kating;
        [SerializeField] private GameObject cam;
        [SerializeField] private Animator emoteAnim;
        [SerializeField] private TextAsset PintuMimpi1;

// -----------------------------------------------------------------------------------------------------------

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Mulai sequence pertama
                NextStep(1);
            }
        }

// -----------------------------------------------------------------------------------------------------------

        protected override void Sequence01()
        {
            Sequence mySequence = DOTween.Sequence(); // Start Sequence
            mySequence.Append(dewa.transform.DOMoveX(7, 1)); // Move Character (Jarak, Waktu)
            mySequence.Append(cam.transform.DOMoveX(7, 1)); // Move Camera (Jarak, Waktu)
            mySequence.PrependInterval(3); // Sebelum animasi sebelumnya
            mySequence.AppendInterval(1); // Wait 1 sec (Setelah animasi sebelumnya)
            mySequence.OnComplete(() => // Menunggu cutscene selesai sebelum dialog 
            {
                StartDialog(PintuMimpi1, emoteAnim); // Mulai dialog
            });
        }

// -----------------------------------------------------------------------------------------------------------

    // private void StartCutscene()
    // {
    //     // Fade in sebelum memulai cutscene
    //     FadeManager.Instance.FadeIn(() => 
    //     {
    //         Debug.Log("Fade in selesai, mulai cutscene...");
    //         NextStep(1); // Mulai cutscene setelah fade in selesai
    //     });
    // }

    // private void EndCutscene()
    // {
    //     // Fade out setelah cutscene selesai
    //     FadeManager.Instance.FadeOut(() => 
    //     {
    //         Debug.Log("Fade out selesai, cutscene berakhir.");
    //     });
    // }

// -----------------------------------------------------------------------------------------------------------

        // protected override void Sequence02()
        // {
        //     Sequence mySequence = DOTween.Sequence();
        //     mySequence.Append(cam.transform.DOMoveX(45, 1));

        //     // Mulai dialog setelah animasi selesai
        //     StartDialog(cutscene1002, emoteAnim);
        // }

        protected override void Sequence03()
        {
            Debug.Log("Sequence 03 dijalankan.");
            // Implementasi sequence 03 jika diperlukan
        }
    }
}