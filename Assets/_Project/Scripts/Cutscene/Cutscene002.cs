using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene002 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;
        [SerializeField] private GameObject ibudewa;
        
        [SerializeField] private GameObject cam;
        [SerializeField] private Animator emoteAnim;
        [SerializeField] private TextAsset RumahBali_AfterOutDewaRoom_02;
        
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
            StartDialog(RumahBali_AfterOutDewaRoom_02, emoteAnim); // Mulai dialog setelah jeda
        }

        protected override void Sequence03()
        {
            Debug.Log("Sequence 03 dijalankan.");
            // Implementasi sequence 03 jika diperlukan
        }
    }
}