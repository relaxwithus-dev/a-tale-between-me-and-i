using System.Collections;
using UnityEngine;
using ATBMI.Entities.Player;
using ATBMI.Gameplay.Controller;

namespace ATBMI.Scene
{
    public class LocalTraveler : Traveler
    {
        // Internal fields
        [Header("Attribute")]
        [SerializeField] private float travelDuration = 1.5f;
        [SerializeField] private Transform targetPoint;
        [SerializeField] private FadeController fader;
        
        private PlayerController _player;

        // Methods
        protected override void InitOnStart()
        {
            base.InitOnStart();
            _player = SceneNavigation.Instance.Player;
        }
        
        protected override void TravelToTarget()
        {
            StartCoroutine(LocalTravelRoutine());
        }
        
        private IEnumerator LocalTravelRoutine()
        {
            fader.FadeOut();
            _player.StopMovement();
            yield return new WaitForSeconds(fader.FadeDuration);
            
            _player.transform.position = targetPoint.position;
            yield return new WaitForSeconds(travelDuration);
            fader.FadeIn(() =>
            { 
                _player.StartMovement();
            });
        }
    }
}