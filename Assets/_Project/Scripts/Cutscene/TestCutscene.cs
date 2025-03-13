using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene01 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;
        // [SerializeField] private GameObject ratna;
        // [SerializeField] private GameObject waffa;
        // [SerializeField] private GameObject luna;
        // [SerializeField] private GameObject kating;
        [SerializeField] private GameObject cam;
        [SerializeField] private Animator emoteAnim;
        [SerializeField] private TextAsset PintuMimpi1;
        // [SerializeField] private TextAsset cutscene1002;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Mulai sequence pertama
                NextStep(1);
            }
        }

        protected override void Sequence01()
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(dewa.transform.DOMoveX(7, 1));
            mySequence.Append(cam.transform.DOMoveX(7, 1));
            // mySequence.PrependInterval(3);
            // mySequence.Append(ratna.transform.DOMoveX(45, 1));
            mySequence.AppendInterval(1); // Jeda 1 detik
            mySequence.OnComplete(() => 
            {
                StartDialog(PintuMimpi1, emoteAnim); // Mulai dialog setelah jeda
            });
        }

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