using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.Enum;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Util.Coroutine;
using Util.Enums;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public UIPageType DefaultUiPage;

        public float MinLoadingScreenTime = 0f;

        private List<UIController> uiControllers;
        private Hashtable uiHashtable;

        private UIPageType _lastActiveUiPage;

        void Awake()
        {
            uiControllers = GetComponentsInChildren<UIController>().ToList();
            uiHashtable = new Hashtable();

            RegisterUIControllers(uiControllers);
        }

        void Update()
        {
            if (Keyboard.current?.escapeKey.wasPressedThisFrame == true ||
                Gamepad.current?.buttonEast.wasPressedThisFrame == true)
                ReturnToPrevious();
        }

        void Start()
        {
            foreach (var controller in uiControllers)
                controller.Disable();

            EnableUI(DefaultUiPage);
        }

        public void ReturnToPrevious() => GetUI(_lastActiveUiPage)?.ReturnToUI();

        public void EnableUI(UIPageType target, bool resetOnSwitch = false, bool fadeIn = false)
        {
            if (target == UIPageType.None) return;

            GetUI(target)?.Enable(resetOnSwitch, fadeIn);
            _lastActiveUiPage = target;
        }

        public IEnumerator EnableUICoroutine(UIPageType target, bool resetOnSwitch = false, bool transition = true)
        {
            if (target == UIPageType.None) yield break;

            _lastActiveUiPage = target;
            yield return GetUI(target)?.EnableCoroutine(resetOnSwitch, transition);
        }

        public void DisableUI(UIPageType target, bool resetOnSwitch = false, bool fadeOut = false)
        {
            if (target == UIPageType.None) return;

            GetUI(target)?.Disable(resetOnSwitch, fadeOut);
        }

        public IEnumerator DisableUICoroutine(UIPageType target, bool resetOnSwitch = false)
        {
            if (target == UIPageType.None) yield break;

            yield return GetUI(target)?.DisableCoroutine(resetOnSwitch);
        }

        public void DisplayUI(UIPageType target, bool fadeIn = false) => EnableUI(target, fadeIn: fadeIn);
        public void HideUI(UIPageType target, bool fadeOut = false) => DisableUI(target, fadeOut: fadeOut);

        public void SwitchUI(UIPageType target, bool resetCurrentOnSwitch = false, bool resetTargetOnSwitch = true, bool transition = true)
        {
            if (_lastActiveUiPage == target) return;

            StartCoroutine(CoroutineUtil.Sequence(
                DisableUICoroutine(_lastActiveUiPage, resetCurrentOnSwitch),
                EnableUICoroutine(target, resetTargetOnSwitch, transition)
                ));

            // DisableUI(_lastActiveUiPage, resetCurrentOnSwitch, fadeOut);
            // EnableUI(target, resetTargetOnSwitch, fadeIn);
            // _lastActiveUiPage = target;
        }

        public void SwitchScene(SceneType scene)
        {
            if (scene == SceneType.None) return;

            LoadingManager.Instance.LoadScene(scene);
        }

        private UIController GetUI(UIPageType uiPageType) => (UIController) uiHashtable[uiPageType];

        private void RegisterUIControllers(IEnumerable<UIController> controllers)
        {
            foreach (var controller in controllers)
            {
                if (!UIExists(controller.UiPageType))
                    uiHashtable.Add(controller.UiPageType, controller);
            }
        }

        private bool UIExists(UIPageType uiPageType) => uiHashtable.ContainsKey(uiPageType);

        IEnumerator LoadingScreen(SceneType scene)
        {
            var minEndTime = Time.time + MinLoadingScreenTime;
            var result = SceneManager.LoadSceneAsync(scene.ToString());
            while (result.isDone == false || Time.time <= minEndTime)
            {
                yield return null;
            }
        }
    }
}
