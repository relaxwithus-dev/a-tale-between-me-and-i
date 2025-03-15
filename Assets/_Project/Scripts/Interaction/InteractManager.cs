using System;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Dialogue;
using ATBMI.Entities.NPCs;
using ATBMI.Gameplay.Handler;
using UnityEngine.Serialization;

namespace ATBMI.Interaction
{
    public class InteractManager : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Properties")] 
        [SerializeField] private bool canInteracted;
        [SerializeField] private bool isInteracted;
        [SerializeField] private GameObject interactSign;
        
        [Header("Area")]
        [SerializeField][MaxValue(3)] private int detectionLimit;
        [SerializeField] private float detectionRadius;
        [SerializeField] private LayerMask targetMask;
        
        private Collider2D[] _hitsNonAlloc;
        
        [Header("Reference")]
        [SerializeField] private InteractHandler interactHandler;
        
        #endregion

        #region MonoBehaviour Callbacks

        private void OnEnable()
        {
            InteractEvent.OnInteracted += cond => isInteracted = cond;
            InteractEvent.OnRestricted += cond => canInteracted = cond;
        }
        
        private void Start()
        {
            _hitsNonAlloc = new Collider2D[detectionLimit];
            interactSign.SetActive(false);
        }
        
        private void Update()
        {
            if (!canInteracted || isInteracted) return;
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
            var numOfHits = Physics2D.OverlapCircleNonAlloc(transform.position, detectionRadius, _hitsNonAlloc, targetMask);
            
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
                        InteractEvent.InteractedEvent(interact: true);
                        
                        if (target as ItemInteract)
                        {
                            if (!target.Validate()) continue;
                            InteractEvent.InteractedEvent(interact: false);
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
                var target = collider2Ds[i];
                var sqrDist = (target.transform.position - origin).sqrMagnitude;
                if (!closest || sqrDist < closestSqrDist)
                {
                    closest = target;
                    closestSqrDist = sqrDist;
                }
            }
            
            return closest;
        }
        
        private void ActivateSignAt(Transform target)
        {
            if (interactSign.activeSelf || DialogueManager.Instance.IsDialoguePlaying) return;
            
            if (target.TryGetComponent<IInteractable>(out var targetInteract))
            {
                var signTransform = interactSign.transform;
                var signPosTrans = targetInteract.GetSignTransform();
                
                if (signPosTrans == null) return;
                
                // Activate sign
                signTransform.parent = signPosTrans;
                signTransform.localPosition = Vector3.zero;
                interactSign.SetActive(true);
            }
        }
        
        private void DeactivateSign()
        {
            if (!interactSign.activeSelf) return;
            
            interactSign.transform.parent = transform;
            interactSign.transform.position = Vector3.zero;
            interactSign.SetActive(false);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

        #endregion
    }
}
