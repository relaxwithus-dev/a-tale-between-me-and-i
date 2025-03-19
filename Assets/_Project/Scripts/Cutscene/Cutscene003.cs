using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene003 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;
        [SerializeField] private GameObject anjing;
        
        [SerializeField] private GameObject cam;
        [SerializeField] private TextAsset HalamRumahBali_AfterOutOfHouse_03;
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
            StartDialog(HalamRumahBali_AfterOutOfHouse_03); // Mulai dialog setelah jeda
            isTriggered = true;
            }
        }
    }
}