using UnityEngine;

namespace UI.Tween
{
    public class MoveTween : BaseTween
    {
        [Header("Move Tween")]
        [SerializeField] private Vector2 _moveFrom = Vector2.zero;
        [SerializeField] private Vector2 _moveTo = Vector2.one;

        [SerializeField] private LeanTweenType _easeType = LeanTweenType.notUsed;
        [SerializeField] private bool _reverseOnOut = true;

        public override void Reset()
        {
            if (ShouldTweenIn() || _reverseOnOut)
                _rectTransform.anchoredPosition = _moveTo;
            else if (ShouldTweenOut())
                _rectTransform.anchoredPosition = _moveFrom;
        }

        public override void Tween()
        {
            _rectTransform.anchoredPosition = _moveFrom;
            LeanTween.move(_rectTransform, _moveTo, _duration)
                .setDelay(_delay + _delayIn)
                .setEase(_easeType)
                .setIgnoreTimeScale(true);
        }

        public override void TweenOut()
        {
            if (ShouldTweenOut() == false)
                return;

            if (!_reverseOnOut)
            {
                Tween();
                return;
            }

            _rectTransform.anchoredPosition = _moveTo;
            LeanTween.move(_rectTransform, _moveFrom, _duration)
                .setDelay(_delay + _delayOut)
                .setEase(_easeType)
                .setIgnoreTimeScale(true);
        }
    }
}