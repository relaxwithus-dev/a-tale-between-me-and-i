using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene001 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;
        [SerializeField] private GameObject ibudewa;
        [SerializeField] private GameObject cam;
        [SerializeField] private TextAsset KamarDewa_FirstDay_01;
        private bool isTriggered; //Bool Trigger
        
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
            if (isTriggered == false)
            {
                StartDialog(KamarDewa_FirstDay_01); // Mulai dialog setelah jeda
                isTriggered = true;
            }
        }

        protected override void Sequence03()
        {
            Debug.Log("Sequence 03 dijalankan.");
            // Implementasi sequence 03 jika diperlukan
        }
    }
}