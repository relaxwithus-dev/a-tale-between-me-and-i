using UnityEngine;
using DG.Tweening;
using ATBMI.Gameplay.Event;
using ATBMI.Dialogue;

namespace ATBMI
{
    public class Cutscene01 : MonoBehaviour
    {
        // [SerializeField] private GameObject dewa;
        // [SerializeField] private Animator emoteAnim;
        // [SerializeField] private GameObject ratna;
        // [SerializeField] private GameObject waffa;
        // [SerializeField] private GameObject luna;
        // [SerializeField] private GameObject kating;

        // [SerializeField] private GameObject cam;

        // [SerializeField] private TextAsset cutscene1001;
        // [SerializeField] private TextAsset cutscene1002;

        // private void Sequence01()
        // {
        //     Sequence mySequence = DOTween.Sequence();
        //     mySequence.Append(dewa.transform.DOMoveX(45, 1));
        //     mySequence.Append(ratna.transform.DOMoveX(45, 1));

        //     // hapus follow cam

        //     mySequence.Append(cam.transform.DOMoveX(45, 1));
        //     DialogueManager.Instance.EnterDialogueMode(cutscene1001, emoteAnim);
        //     DialogueManager.Instance.TurnSequenceState(true, this);

        //     // gimana cara dia wait dialogue lalu lanjut sequence
        // }

        // private void Sequence02()
        // {
        //     Sequence mySequence = DOTween.Sequence();
        //     mySequence.Append(cam.transform.DOMoveX(45, 1));
        //     DialogueManager.Instance.EnterDialogueMode(cutscene1002, emoteAnim);
        //     DialogueManager.Instance.TurnSequenceState(true, this);
        // }

        // private void OnTriggerEnter2D(Collider2D other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         Sequence01();
        //     }
        // }

        // public void nextstep(int currentstep = 1)
        // {
        //     switch (currentstep)
        //     {
        //         case 01:
        //             Sequence01();
        //             break;
        //         case 02:
        //             Sequence02();
        //             break;
        //         case 03:
        //             Sequence03();
        //             break;
        //         default:
        //     }
        // }
    }
}
