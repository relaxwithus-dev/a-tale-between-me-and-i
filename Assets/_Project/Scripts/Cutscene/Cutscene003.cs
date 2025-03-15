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
        [SerializeField] private Animator emoteAnim;
        [SerializeField] private TextAsset HalamRumahBali_AfterOutOfHouse_03;
        
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
            StartDialog(HalamRumahBali_AfterOutOfHouse_03, emoteAnim); // Mulai dialog setelah jeda
        }
    }
}