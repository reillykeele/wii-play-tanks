using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;
using Util.Enums;

namespace UI.Tween
{
    [RequireComponent(typeof(Image))]
    public class FadeImageTween : BaseTween
    {
        [Header("Fade Image Tween")]
        [SerializeField] private float _fadeFrom = 0f;
        [SerializeField] private float _fadeTo = 1f;

        [SerializeField] private LeanTweenType _easeType = LeanTweenType.notUsed;

        private Image _image;

        protected override void Awake()
        {
            base.Awake();
            _image = GetComponent<Image>();
        }

        public override void Tween()
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _fadeFrom);
            LeanTween.value(_image.gameObject, _fadeFrom, _fadeTo, _duration)
                .setOnUpdate(SetAlphaOnUpdate)
                .setDelay(_delay + _delayIn)
                .setEase(_easeType)
                .setIgnoreTimeScale(true);
        }

        public override void TweenOut()
        {
            if (ShouldTweenOut() == false)
                return;

            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _fadeTo);
            LeanTween.value(gameObject, _fadeTo, _fadeFrom, _duration)
                .setOnUpdate(SetAlphaOnUpdate)
                .setDelay(_delay + _delayOut)
                .setEase(_easeType)
                .setIgnoreTimeScale(true);
        }

        private void SetAlphaOnUpdate(float a) => _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, a);
    }
}