using UnityEngine;
using Util.Enums;

namespace UI.Tween
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class BaseTween : MonoBehaviour
    {
        [Header("Tween")]
        [SerializeField] 
        [Tooltip("The length of tween animation.")]
        protected float _duration = 1f;
        
        [SerializeField]
        [Tooltip("The global delay for before tweening in and out.")]
        protected float _delay = 0f;

        [SerializeField]
        [Tooltip("The delay for before tweening in.")]
        protected float _delayIn = 0f;

        [SerializeField]
        [Tooltip("The delay for before tweening out.")]
        protected float _delayOut = 0f;

        // [SerializeField]
        // [Tooltip("The delay for before tweening in.")]
        // protected float _delayIn = 0f;

        [SerializeField]
        [Tooltip("The delay after tweening out, before the next tween comes in.")]
        protected float _waitOut = 0f;

        [SerializeField]
        [Tooltip("Whether we should cancel the tween to animate another animation.")]
        protected bool _cancelOnTween = true;
        
        [SerializeField]
        [Tooltip("Whether the tween animation should be played OnEnable.")]
        protected bool _tweenInOnEnable = false;
        
        [SerializeField]
        [Tooltip("Whether the tween should tween in, out, or both.")]
        protected TweenDirection _tweenDirection = TweenDirection.In;

        // public UnityEvent OnCompletedEvent;

        protected RectTransform _rectTransform;

        protected virtual void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        protected virtual void OnEnable()
        {
            if (_tweenDirection == TweenDirection.Out)
                Reset();

            if (_tweenInOnEnable)
            {
                TweenIn();
            }
        }

        public abstract void Reset();

        public abstract void Tween();
        public virtual void TweenIn()
        {
            if (_cancelOnTween && LeanTween.isTweening(_rectTransform))
                LeanTween.cancel(_rectTransform);

            if (ShouldTweenIn())
                Tween();
        }

        public virtual void TweenOut()
        {
            if (_cancelOnTween && LeanTween.isTweening(_rectTransform))
                LeanTween.cancel(_rectTransform);

            if (ShouldTweenOut())
                Tween();
        }

        public bool ShouldTweenInOnEnable() => _tweenInOnEnable;
        public bool ShouldTweenIn() => _tweenDirection == TweenDirection.In || _tweenDirection == TweenDirection.InAndOut;
        public bool ShouldTweenOut() => _tweenDirection == TweenDirection.Out || _tweenDirection == TweenDirection.InAndOut;

        public float GetDelay() => _delay;
        
        public float GetDuration() => _duration + _delay;
        public float GetDurationIn() => _duration + _delay + _delayIn;
        public float GetDurationOut() => _duration + _delay + _delayOut + _waitOut;

        public void SetDelay(float delay) => _delay = delay;
        public void SetDelayIn(float delay) => _delayIn = delay;
        public void SetDelayOut(float delay) => _delayOut = delay;
    }
}
