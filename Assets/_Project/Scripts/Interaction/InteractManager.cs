using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Utilities;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Interaction
{
    /// <summary>
    /// InteractManager buat handle interaksi
    /// karakter player dengan object in-game.
    /// </summary>
    public class InteractManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Neccesary")]
        [SerializeField] private bool isInteracted;
        [SerializeField] private GameObject interactSign;

        private readonly float signYMultiplier = 1f;
        
        public bool IsInteracted
        {
            get => isInteracted;
            set => isInteracted = value;
        }
        
        [Header("Area")]
        [SerializeField] [MaxValue(3)] private int detectionLimit;
        [SerializeField] private Vector2 boxSize;
        [SerializeField] private LayerMask targetMask;

        private Collider2D[] hitsNonAlloc;

        [Header("Reference")]
        [SerializeField] private InteractHandler interactHandler;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            hitsNonAlloc = new Collider2D[detectionLimit];
            interactSign.SetActive(false);
        }

        private void Update()
        {
            if (IsInteracted) return;
            HandleInteractArea();
        }
        
        #endregion

        #region Methods
        
        // !- Core
        private void HandleInteractArea()
        {
            // Alloc intersection
            var numOfHits = Physics2D.OverlapBoxNonAlloc(transform.position, boxSize, 0f, hitsNonAlloc, targetMask);
            
            if (numOfHits == 0)
                DeactivateSign();

            for (var i = 0; i < numOfHits; i++)
            {
                var hits = hitsNonAlloc[i];
                if (hits.CompareTag(GameConstant.NPC_TAG) || hits.CompareTag(GameConstant.NPC_TAG))
                {
                    var nearest = FindNearestObjectAt(transform.position, numOfHits, hitsNonAlloc);
                    if (nearest != null)
                    {
                        // Sign
                        var nearestTransform = nearest.transform;
                        ActivateSignAt(nearestTransform);

                        // Interact
                        if (GameInputHandler.Instance.IsTapInteract)
                        {
                            var target = nearest.GetComponent<Interaction>();
                            if (target != null)
                            {
                                IsInteracted = true;
                                DeactivateSign();

                                if (target as ItemInteraction)
                                {
                                    isInteracted = false;
                                    target.Interact(this);
                                }
                                else
                                {
                                    interactHandler.OpenInteractOption(target);
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private Collider2D FindNearestObjectAt(Vector3 origin, int hitNums, Collider2D[] collider2Ds)
        {
            float closestSqrDist = Mathf.Infinity;
            Collider2D closest = null;

            for (var i = 0; i < hitNums; i++)
            {
                var collider = collider2Ds[i];
                var sqrDist = (collider.transform.position - origin).sqrMagnitude;
                if (!closest || sqrDist < closestSqrDist)
                {
                    closest = collider;
                    closestSqrDist = sqrDist;
                }
            }
            
            return closest;
        }

        private void ActivateSignAt(Transform target)
        {
            if (interactSign.activeSelf) return;

            var targetPos = target.position;
            interactSign.transform.position = new Vector3(targetPos.x, targetPos.y + signYMultiplier, targetPos.z);
            interactSign.transform.parent = target;
            interactSign.SetActive(true);
        }

        private void DeactivateSign()
        {
            if (!interactSign.activeSelf) return;

            interactSign.transform.position = Vector3.zero;
            interactSign.transform.parent = transform;
            interactSign.SetActive(false);
        }

        // !- Helpers
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, boxSize);
        }

        #endregion
    }
}
