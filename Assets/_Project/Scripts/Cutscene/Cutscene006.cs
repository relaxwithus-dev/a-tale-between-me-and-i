using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene006 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;
        [SerializeField] private GameObject ibudewa;
        
        [SerializeField] private GameObject cam;
        [SerializeField] private TextAsset KamarDewa_SecondDay_07;
        
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
            StartDialog(KamarDewa_SecondDay_07);
        }
    }
}