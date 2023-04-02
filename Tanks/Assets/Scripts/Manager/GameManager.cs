using System;
using System.Collections;
using Actor;
using Data;
using Data.Enum;
using ScriptableObject.Config;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util.Coroutine;
using Util.Enums;
using Util.Singleton;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Configuration")]
        public GameConfigScriptableObject Config;

        [Header("Game State")]
        public UnityEvent OnPauseGameEvent;
        public UnityEvent OnResumeGameEvent;
        private GameState _currentGameState = GameState.Playing;
        public GameState CurrentGameState
        {
            get => _currentGameState;
            set => _currentGameState = value;
        }

        [Header("Game Mode")]
        public GameMode GameMode = GameMode.None;

        // Audio
        // TODO: Should this be elsewhere?
        [Header("Audio")]
        public AudioMixer Mixer;

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(transform.root.gameObject);

            if (FindObjectOfType<LoadingManager>() == null)
                SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);
        }

        public bool IsPlaying() => CurrentGameState == GameState.Playing;
        public bool IsPaused() => CurrentGameState == GameState.Paused;

        public void PauseGame()
        {
            if (CurrentGameState != GameState.Playing) return;

            CurrentGameState = GameState.Paused;
            OnPauseGameEvent.Invoke();

            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            if (CurrentGameState != GameState.Paused) return;

            CurrentGameState = GameState.Playing;
            OnResumeGameEvent.Invoke();

            Time.timeScale = 1;
        }

        public void TogglePaused() { if (IsPlaying()) PauseGame(); else ResumeGame(); }

        public void Quit()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit(0);
            #endif
        }

        // Game-specific logic
        // TODO: Consider EXTENDING this class as TankGameManager
        // TODO: and moving GameManager to Util.

        public UnityEvent LevelStartEvent = new UnityEvent();
        public IEnumerator StartLevelCoroutine() => CoroutineUtil.Sequence(
                CoroutineUtil.Wait(7),
                CoroutineUtil.CallAction(() => LevelStartEvent.Invoke()),
                CoroutineUtil.CallAction(() => CurrentGameState = GameState.Playing)
            );

        public void LoadLevel(LevelData level)
        {
            StartCoroutine(CoroutineUtil.Sequence(
                CoroutineUtil.CallAction(() => CurrentGameState = GameState.Cutscene),
                LoadingManager.Instance.LoadSceneCoroutine(level.SceneType, true),
                CoroutineUtil.CallAction(() => TransitionLevelEvent.Invoke(level)),
                CoroutineUtil.Wait(0.5f), // TODO This is really janky...
                LoadingManager.Instance.SetLoading(false),
                StartLevelCoroutine()
            ));
        }

        public UnityEvent LevelClearEvent = new UnityEvent();
        public void LevelClear(LevelData nextLevel)
        {
            StartCoroutine(CoroutineUtil.Sequence(
                CoroutineUtil.CallAction(() => CurrentGameState = GameState.Cutscene),
                CoroutineUtil.CallAction(() => LevelClearEvent.Invoke()),
                CoroutineUtil.Wait(5),
                CoroutineUtil.CallAction(() =>
                {
                    if (nextLevel == null)
                        LoadingManager.Instance.LoadScene(SceneType.Menu);
                    else
                        TransitionLevel(nextLevel);
                })
            ));
        }

        public UnityEvent<LevelData> TransitionLevelEvent = new UnityEvent<LevelData>();
        public void TransitionLevel(LevelData nextLevel)
        {
            StartCoroutine(CoroutineUtil.Sequence(
                CoroutineUtil.CallAction(() => TransitionLevelEvent.Invoke(nextLevel)),
                CoroutineUtil.Wait(1),
                CoroutineUtil.CallAction(() => LoadingManager.Instance.TransitionScene(nextLevel.SceneType)),
                StartLevelCoroutine()
            ));
        }

        
    }
}
