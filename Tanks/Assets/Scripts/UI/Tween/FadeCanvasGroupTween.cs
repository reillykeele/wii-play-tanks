using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Util.Enums;

namespace UI.Tween
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeCanvasGroupTween : BaseTween
    {
        [Header("Fade Canvas Group Tween")]
        [SerializeField] private float _fadeFrom = 0f;
        [SerializeField] private float _fadeTo = 1f;

        [SerializeField] private LeanTweenType _easeType = LeanTweenType.notUsed;
        [SerializeField] private bool _reverseOnOut = true;

        private CanvasGroup _canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Reset()
        {
            if (ShouldTweenIn() || _reverseOnOut)
                _canvasGroup.alpha = _fadeTo;
            else if (ShouldTweenOut())
                _canvasGroup.alpha = _fadeFrom;
        }

        public override void Tween()
        {
            _canvasGroup.alpha = _fadeFrom;
            LeanTween.value(_canvasGroup.gameObject, _fadeFrom, _fadeTo, _duration)
                .setOnUpdate(SetAlphaOnUpdate)
                .setDelay(_delay + _delayIn)
                .setEase(_easeType)
                .setIgnoreTimeScale(true);
        }

        public override void TweenOut()
        {
            if (_tweenDirection != TweenDirection.Out && _tweenDirection != TweenDirection.InAndOut)
                return;

            if (!_reverseOnOut)
            {
                Tween();
                return;
            }

            _canvasGroup.alpha = _fadeTo;
            LeanTween.value(gameObject, _fadeTo, _fadeFrom, _duration)
                .setOnUpdate(SetAlphaOnUpdate)
                .setDelay(_delay + _delayOut)
                .setEase(_easeType)
                .setIgnoreTimeScale(true);
        }

        private void SetAlphaOnUpdate(float a) => _canvasGroup.alpha = a;
    }
}