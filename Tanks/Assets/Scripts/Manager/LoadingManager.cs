using System.Collections;
using Data.Enum;
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

        /// <summary>
        /// Unloads the active scene and loads the <c>toScene</c> scene, setting it to active.
        /// </summary>
        /// <param name="toScene">The scene to load in.</param>
        public void TransitionScene(SceneType toScene)
        {
            // Unload the active scene
            var unloadOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            // Load the next scene
            unloadOp.completed += (op) =>
            {
                var loadOp = SceneManager.LoadSceneAsync(toScene.ToString(), LoadSceneMode.Additive);
                loadOp.completed += (o2) => SceneManager.SetActiveScene(SceneManager.GetSceneByName(toScene.ToString()));
            };
        }

        /// <summary>
        ///  Unloads the <c>fromScene</c> scene and loads the <c>toScene</c> scene, setting it to active.
        /// </summary>
        /// <param name="fromScene">The scene to unload.</param>
        /// <param name="toScene">The scene to load in.</param>
        public void TransitionScene(SceneType fromScene, SceneType toScene)
        {
            // Unload the current level
            var unloadOp = SceneManager.UnloadSceneAsync(fromScene.ToString());

            // Load the next level
            unloadOp.completed += (op) =>
            {
                var loadOp = SceneManager.LoadSceneAsync(toScene.ToString(), LoadSceneMode.Additive);
                loadOp.completed += (o2) => SceneManager.SetActiveScene(SceneManager.GetSceneByName(toScene.ToString()));
            };
        }

        /// <summary>
        /// Unloads all currently loaded scenes to load in the specified scene. Displays a loading screen while
        /// loading in the new scene.
        /// </summary>
        /// <param name="scene">The scene to load in.</param>
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
            result.completed += (op) => SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.ToString()));

            // wait
            while (result.isDone == false || Time.time <= minEndTime)
            {
                yield return null;
            }
        }

    }
}
