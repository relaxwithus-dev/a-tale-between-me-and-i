using UnityEngine;
using DG.Tweening;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene005 : CutsceneManager
    {
        [SerializeField] private GameObject dewa;      
        [SerializeField] private GameObject cam;
        [SerializeField] private Animator emoteAnim;
        [SerializeField] private TextAsset KamarDewa_AfterBackTebing_06;
        
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
            StartDialog(KamarDewa_AfterBackTebing_06, emoteAnim);
        }
    }
}