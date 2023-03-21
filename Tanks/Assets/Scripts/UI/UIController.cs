using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.ButtonControllers;
using UI.Tween;
using Util.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util.Enums;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIController : MonoBehaviour
    {
        public UIPageType UiPageType;
        public UIPageType ReturnUiPage;

        public Selectable initialSelected = null;

        private CanvasGroup _canvasGroup;
        protected CanvasController _canvasController;
        protected CanvasAudioController _canvasAudioController;
        protected List<ASelectableController> _selectableControllers;

        protected Selectable lastSelected = null;

        // Children
        protected IEnumerable<BaseTween> _tweens;
        protected IEnumerable<Animator> _animators;

        protected virtual void Awake()
        {
            _canvasController = GetComponentInParent<CanvasController>();
            _canvasAudioController = GetComponentInParent<CanvasAudioController>();
            _selectableControllers = GetComponentsInChildren<ASelectableController>().ToList();

            _tweens = GetComponentsInChildren<BaseTween>();
            _animators = GetComponentsInChildren<Animator>();

            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void Reset()
        {
            lastSelected = null;
        }

        public virtual void Enable(bool resetOnSwitch = false, bool fadeIn = false)
        {
            if (resetOnSwitch)
                Reset();

            if (lastSelected != null)
                lastSelected.Select();
            else if (initialSelected != null)
                initialSelected.Select();
            else
                _selectableControllers?.FirstOrDefault()?.Select();

            gameObject.Enable();

            // if (fadeIn)
            //     StartCoroutine(UIHelper.FadeIn(_canvasGroup));
            // else if (_canvasGroup != null)
            //     _canvasGroup.alpha = 1f;

            // if (_animators != null)
            //     foreach (var animator in _animators)
            //         animator.TrySetBool("transitionIn", resetOnSwitch);
        }

        public virtual IEnumerator EnableCoroutine(bool resetOnSwitch = false, bool transition = true)
        {
            if (resetOnSwitch)
                Reset();

            if (lastSelected != null)
                lastSelected.Select();
            else if (initialSelected != null)
                initialSelected.Select();
            else
                _selectableControllers?.FirstOrDefault()?.Select();


            if (transition)
                foreach (var tween in _tweens.Where(x =>/* x.gameObject.activeInHierarchy && */ x.ShouldTweenInOnEnable() == false && x.ShouldTweenIn()))
                    tween.TweenIn();

            gameObject.Enable();

            yield break;
        }

        public virtual void Disable(bool resetOnSwitch = false, bool fadeOut = false)
        {
            lastSelected = EventSystem.current?.currentSelectedGameObject?.GetComponent<Selectable>();

            gameObject.Disable();
        }

        public virtual IEnumerator DisableCoroutine(bool resetOnSwitch = false)
        {
            lastSelected = EventSystem.current?.currentSelectedGameObject?.GetComponent<Selectable>();

            var transitionDuration = 0f;
            foreach (var tween in _tweens.Where(x => x.gameObject.activeInHierarchy && x.ShouldTweenOut()))
            {
                transitionDuration = Mathf.Max(transitionDuration, tween.GetDurationOut());
                tween.TweenOut();
            }

            yield return new WaitForSecondsRealtime(transitionDuration);

            gameObject.Disable();
        }

        public virtual void ReturnToUI()
        {
            if (ReturnUiPage != UIPageType.None)
            {
                _canvasAudioController.Play(CanvasAudioController.CanvasAudioSoundType.Back);
                _canvasController.SwitchUI(ReturnUiPage, resetTargetOnSwitch: false, transition: true);
            }
        }

        public IEnumerator FadeIn()
        {
            _canvasGroup.alpha = 0f;

            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha += 0.05f;
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }

        public IEnumerator FadeOut()
        {
            _canvasGroup.alpha = 1f;

            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha -= 0.05f;
                yield return null;
            }

            _canvasGroup.alpha = 0f;
        }
    }
}
