using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene004 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;
        [SerializeField] private GameObject anjing; 
        [SerializeField] private GameObject cam;
        [SerializeField] private TextAsset Tebing_Monolog1_04;
        [SerializeField] private TextAsset Tebing_Monolog2_05;
        private bool isTriggered; //Bool Trigger
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Mulai sequence pertama
                NextStep(1);
            }
        }

        private void Awake()
        {
            if (!DOTween.IsTweening(cam.transform))
            {
                Debug.Log("DOTween tidak aktif, menginisialisasi ulang...");
                DOTween.Init();
            }
        }
        protected override void Sequence01()
        {
            if (isTriggered == false)
            {
            Sequence mySequence = DOTween.Sequence(); // Start Sequence
            StartDialog(Tebing_Monolog1_04); // Mulai dialog setelah jeda
            mySequence.Play(); // Pastikan Sequence dimainkan!
            isTriggered = true;
            }
        }

        protected override void Sequence02()
        {
            if (isTriggered == false)
            {
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(cam.transform.DOMoveY(0, 1)); // Move Camera (Jarak, Waktu)
            mySequence.AppendInterval(1); // Wait 1 sec (Setelah animasi sebelumnya)
            mySequence.Append(cam.transform.DOMoveY(-4, 1));
            mySequence.AppendCallback(() => 
            {
                StartDialog(Tebing_Monolog2_05); // Mulai dialog setelah jeda
            });

            mySequence.Play(); // Pastikan Sequence dimainkan!
            isTriggered = true;
            }
        }
    }
}