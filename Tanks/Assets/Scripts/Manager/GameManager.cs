using System;
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
        // TODO: Figure out a better place or system to do this
        public UnityEvent<LevelData> LevelClearEvent = new UnityEvent<LevelData>();
        public void LevelClear(LevelData nextLevel)
        {
            StartCoroutine(CoroutineUtil.Sequence(
                CoroutineUtil.CallAction(() => LevelClearEvent.Invoke(nextLevel)),
                CoroutineUtil.Wait(5),
                CoroutineUtil.CallAction(() => TransitionLevel(nextLevel.SceneType))
            ));
        }

        public UnityEvent TransitionLevelEvent = new UnityEvent();
        public void TransitionLevel(SceneType nextlevel)
        {
            StartCoroutine(CoroutineUtil.Sequence(
                CoroutineUtil.CallAction(() => TransitionLevelEvent.Invoke()),
                CoroutineUtil.Wait(1),
                CoroutineUtil.CallAction(() => LoadingManager.Instance.TransitionScene(nextlevel))
            ));
        }

    }
}
