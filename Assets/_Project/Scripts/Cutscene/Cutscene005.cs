using UnityEngine;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene005 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;      
        [SerializeField] private GameObject cam;
        [SerializeField] private TextAsset KamarDewa_AfterBackTebing_06;

        protected override void Start()
        {
            base.Start(); // Panggil Start() dari CutsceneManager
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !cutsceneTriggered)
            {
                // Mulai sequence pertama
                NextStep(1);
                MarkCutsceneAsTriggered(); // Tandai cutscene telah dimainkan
            }
        }

        protected override void Sequence01()
        {
            StartDialog(KamarDewa_AfterBackTebing_06);
        }
    }
}