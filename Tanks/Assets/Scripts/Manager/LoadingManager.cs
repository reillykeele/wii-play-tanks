using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util.Coroutine;
using Util.Enums;
using Util.Helpers;
using Util.Singleton;

namespace Manager
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        public bool IsLoading { get; private set; }

        public float MinLoadingScreenTime = 0f;

        private UIController _uiController;
        private CanvasGroup _loadingCanvasGroup;

        public UnityEvent OnSceneLoadedEvent = new UnityEvent();

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(transform.root.gameObject);

            _uiController = GetComponentInChildren<UIController>();
            _loadingCanvasGroup = GetComponentInChildren<CanvasGroup>();

            _uiController?.Disable();
        }

        void Start()
        {
            OnSceneLoadedEvent.Invoke();
        }

        public void QuitGame()
        {
            StartCoroutine(CoroutineUtil.Sequence(
                UIHelper.FadeInAndEnable(_uiController, _loadingCanvasGroup),
                CoroutineUtil.CallAction(() => GameManager.Instance.Quit()))
            );
        }

        public void LoadScene(SceneType scene)
        {
            StartCoroutine(CoroutineUtil.Sequence(
                SetLoading(true),
                UIHelper.FadeInAndEnable(_uiController, _loadingCanvasGroup),
                LoadingScreen(scene),
                UIHelper.FadeOutAndDisable(_uiController, _loadingCanvasGroup),
                SetLoading(false),
                CoroutineUtil.CallAction(() => OnSceneLoadedEvent.Invoke()))
            );
        }

        private IEnumerator SetLoading(bool isLoading)
        {
            IsLoading = isLoading;
            yield break;
        }

        private IEnumerator LoadingScreen(SceneType scene)
        {
            Time.timeScale = 1;

            var minEndTime = Time.time + MinLoadingScreenTime;
            var result = SceneManager.LoadSceneAsync(scene.ToString());
            while (result.isDone == false || Time.time <= minEndTime)
            {
                yield return null;
            }
        }

        // public void LoadUI(string uiSceneName)
        // {
        //     if (SceneManager.GetSceneByName(uiSceneName).isLoaded == false)
        //         StartCoroutine()
        // }
        //
        // private IEnumerator LoadUICoroutine(string uiSceneName)
        // {
        //
        // }
    }
}
