using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using ATBMI.Audio;
using ATBMI.Dialogue;
using ATBMI.Entities;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Interaction
{
    public class InteractManager : MonoBehaviour
    {
        #region Fields & Properties
        
        [Header("Properties")] 
        [SerializeField] private bool isInteracted;
        [SerializeField] private bool isRestricted;
        [SerializeField] private GameObject interactSign;
        
        [Header("Area")]
        [SerializeField][MaxValue(3)] private int detectionLimit;
        [SerializeField] private Transform centerPoint;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask targetMask;
        
        private Collider2D[] _hitsNonAlloc;
        
        [Header("Reference")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerAnimation playerAnimation;
        [SerializeField] private InteractHandler interactHandler;
        
        #endregion

        #region Methods

        // Unity Callbacks
        private void OnEnable()
        {
            InteractEvent.OnInteracted += HandleInteracted;
            InteractEvent.OnRestricted += HandleRestricted;
        }
        
        private void OnDisable()
        {
            InteractEvent.OnInteracted -= HandleInteracted;
            InteractEvent.OnRestricted -= HandleRestricted;
        }
        
        private void Start()
        {
            _hitsNonAlloc = new Collider2D[detectionLimit];
            interactSign.SetActive(false);
        }
        
        private void Update()
        {
            if (isInteracted) return;
            HandleInteractArea();
        }
        
        // Core
        private void HandleInteractArea()
        {
            if (DialogueManager.Instance.IsDialoguePlaying || isRestricted)
            {
                DeactivateSign();
                return;
            }
            
            // Alloc intersection
            var numOfHits = Physics2D.OverlapCircleNonAlloc(transform.position, radius, _hitsNonAlloc, targetMask);
            
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
                        InteractEvent.InteractedEvent(interact: true, playerController);
                        AudioManager.Instance.PlayAudio(Musics.SFX_Interact);
                        
                        if (target is ItemInteract item)
                        {
                            if (!target.Validate()) continue;
                            StartCoroutine(TakeItemRoutine(item));
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

        private IEnumerator TakeItemRoutine(ItemInteract item)
        {
            var duration = playerAnimation.GetAnimationTime();
            playerAnimation.TrySetAnimationState(StateTag.TAKE_ITEM_STATE);
            
            yield return new WaitForSeconds(duration);
            InteractEvent.InteractedEvent(interact: false, playerController);
            item.Interact();
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

        private void HandleInteracted(bool isInteract, PlayerController player)
        {
            isInteracted = isInteract;
            if (isInteracted)
                player.StopMovement();
            else
                player.StartMovement();
        }
        private void HandleRestricted(bool isRestrict) => isRestricted = isRestrict;

        #endregion
    }
}
