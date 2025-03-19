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
        private bool isTriggered; //Bool
        
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
            StartDialog(JalanBali_AfterParentEncounter_08);
            isTriggered = true;
            }
        }
    }
}