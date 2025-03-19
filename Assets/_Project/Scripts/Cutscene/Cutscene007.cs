using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene007 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;     
        [SerializeField] private GameObject cam;
        [SerializeField] private TextAsset JalanBali_AfterParentEncounter_08;
        
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
            StartDialog(JalanBali_AfterParentEncounter_08);
        }
    }
}