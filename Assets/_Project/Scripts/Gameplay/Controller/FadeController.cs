using UnityEngine;
using DG.Tweening;

namespace ATBMI.Gameplay.Controller
{
    public class FadeController : MonoBehaviour
    {
        #region Fields & Properties

        [Header("Stats")]
        [SerializeField][Range(0f, 3f)] private float fadeDuration = 0.5f;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private Tween _fadeTween;
        public float FadeDuration => fadeDuration;
        
        #endregion

        #region Methods
        
        // Unity Callbacks
        private void OnEnable()
        {
            DOTween.Init(true, false, LogBehaviour.Verbose).SetCapacity(900, 450);
        }
        
        // Core
        public void FadeIn()
        {
            canvasGroup.gameObject.SetActive(true);
            
            DoFade(1f, 0f);
            DoFade(0f, fadeDuration, () => 
            {
                canvasGroup.interactable = true;
                canvasGroup.gameObject.SetActive(false);
            });
        }
        
        public void FadeOut()
        {
            canvasGroup.gameObject.SetActive(true);

            DoFade(0f, 0f);
            DoFade(1f, fadeDuration, () =>
            {
                canvasGroup.interactable = false;
            });
        }

        public void DoFade(float target, float duration, TweenCallback callback = null)
        {
            _fadeTween?.Kill(false);
            _fadeTween = canvasGroup.DOFade(target, duration);
            _fadeTween.onComplete += callback;
        }

        #endregion
    }
}
