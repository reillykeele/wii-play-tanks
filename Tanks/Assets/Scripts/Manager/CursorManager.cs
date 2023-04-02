using Assets.Scripts.ScriptableObject;
using UnityEngine;
using Util.Enums;
using Util.Singleton;

namespace Manager
{
    public class CursorManager : Singleton<CursorManager>
    {
        [Header("Configuration")] 
        [SerializeField] private CursorMode CursorMode = CursorMode.ForceSoftware;
        [SerializeField] private CursorLockMode LockMode = CursorLockMode.None;
        [SerializeField] private bool DisplayCursorOnLoadScreen = false;

        [Header("Cursor Sprites")]
        [SerializeField] private CursorScriptableObject _menuCursor;
        [SerializeField] private CursorScriptableObject _crosshairCursor;

        // private bool _isVisible = true;

        void Start()
        {
            Cursor.lockState = LockMode;

            if (DisplayCursorOnLoadScreen == false)
            {
                LoadingManager.Instance.OnLoadStartEvent.AddListener(HideCursor);
                LoadingManager.Instance.OnLoadEndEvent.AddListener(ShowCursor);
            }

            GameManager.Instance.OnGameStateChangeEvent.AddListener(OnGameStateChanged);

            OnGameStateChanged(GameManager.Instance.CurrentGameState);
        }

        public void ShowCursor()
        {
            Cursor.visible = true;
        }

        public void HideCursor()
        {
            Cursor.visible = false;
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state is GameState.Menu or GameState.Paused)
            {
                // Display menu cursor
                SetCursor(_menuCursor);
            }
            else if (state is GameState.Playing or GameState.Cutscene)
            {
                // Display crosshair
                SetCursor(_crosshairCursor);
            }
        }

        private void SetCursor(CursorScriptableObject cursor)
        {
            Cursor.SetCursor(cursor.CursorTexture2D, cursor.GetHotspot(), CursorMode);
        }
        
    }
}
