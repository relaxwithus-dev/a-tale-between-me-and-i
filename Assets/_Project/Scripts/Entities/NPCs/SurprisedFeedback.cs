using UnityEngine;
using DG.Tweening;

namespace ATBMI.Entities.NPCs
{
    public class SurprisedFeedback : MonoBehaviour
    {
        [SerializeField] private float jumpPower;
        [SerializeField] private int jumpNum;
        [SerializeField] private float jumpDuration;
        [SerializeField] private float jumpDistance;

        private Vector3 _jumpTarget;
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpTarget = new Vector3(transform.position.x + jumpDistance,
                    transform.position.y,
                    transform.position.z);
                
                transform.DOJump(_jumpTarget, jumpPower, jumpNum, jumpDuration).SetEase(Ease.OutSine);
            }
        }
    }
}
