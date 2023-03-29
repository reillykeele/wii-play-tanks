using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Tween;
using UnityEngine;
using Util.Helpers;

namespace UI.UIElementControllers
{
    public class TimedTweenOutUIElementController : MonoBehaviour
    {
        public bool DisableOnAwake = true;
        public float TimeBeforeTransition = 1f;

        private bool _isInitialized = false;

        private List<BaseTween> _tweens;

        void Awake()
        {
            _tweens = GetComponents<BaseTween>().ToList();

            if (DisableOnAwake)
            {
                gameObject.Disable();
                _isInitialized = true;
            }
        }

        public void OnEnable()
        {
            // Do not run when we are first initialized, only when we are turned on
            if (_isInitialized == false)
            {
                _isInitialized = true;
                return;
            }

            Invoke("TweenOut", TimeBeforeTransition);
        }

        private void TweenOut() => StartCoroutine(DisableCoroutine());

        private IEnumerator DisableCoroutine()
        {
            var transitionDuration = 0f;
            foreach (var tween in _tweens.Where(x => x.gameObject.activeInHierarchy && x.ShouldTweenOut()))
            {
                transitionDuration = Mathf.Max(transitionDuration, tween.GetDurationOut());
                tween.TweenOut();
            }

            yield return new WaitForSecondsRealtime(transitionDuration);

            gameObject.Disable();
        }
    }
}