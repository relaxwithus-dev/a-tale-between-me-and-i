using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Dialogue;
using ATBMI.Gameplay.Handler;

namespace ATBMI.Interaction
{
    public class InteractManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Properties")]
        [SerializeField] private bool isInteracted;
        [SerializeField] private GameObject interactSign;
        [SerializeField] private float signYMultiplier = 2f;
        
        public bool IsInteracted
        {
            get => isInteracted;
            set => isInteracted = value;
        }
        
        [Header("Area")]
        [SerializeField][MaxValue(3)] private int detectionLimit;
        [SerializeField] private Vector2 boxSize;
        [SerializeField] private LayerMask targetMask;

        private Collider2D[] _hitsNonAlloc;

        [Header("Reference")]
        [SerializeField] private InteractHandler interactHandler;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _hitsNonAlloc = new Collider2D[detectionLimit];
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
            if (DialogueManager.Instance.IsDialoguePlaying)
            {
                DeactivateSign();
                return;
            }

            // Alloc intersection
            var numOfHits = Physics2D.OverlapBoxNonAlloc(transform.position, boxSize, 0f, _hitsNonAlloc, targetMask);
            if (numOfHits == 0)
                DeactivateSign();

            for (var i = 0; i < numOfHits; i++)
            {
                var nearest = FindNearestObjectAt(transform.position, numOfHits, _hitsNonAlloc);
                if (!nearest) continue;
                
                // Sign
                var nearestTransform = nearest.transform;
                ActivateSignAt(nearestTransform);
                
                // Interact
                if (GameInputHandler.Instance.IsTapInteract)
                {
                    var target = nearest.GetComponent<IInteractable>();
                    if (target != null)
                    {
                        DeactivateSign();
                        IsInteracted = true;
                        CharacterInteract.InteractingEvent(isBegin: true);
                        
                        if (target as ItemInteract)
                        {
                            if (!target.Validate()) continue;
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
            if (interactSign.activeSelf || DialogueManager.Instance.IsDialoguePlaying) return;

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
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, boxSize);
        }

        #endregion
    }
}
