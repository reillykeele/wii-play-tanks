using Data.Enum;
using Manager;
using UnityEngine.InputSystem;
using Util.Enums;

namespace UI.UIControllers
{
    public class StartMenuUIController : UIController
    {
        public UIPageType TargetUiPageType = UIPageType.MainMenu;

        protected override void Awake()
        {
            base.Awake();

            GameManager.Instance.CurrentGameState = GameState.Menu;
        }

        void Update()
        {
            // TODO: Use Input System
            if (Keyboard.current?.anyKey.wasPressedThisFrame == true ||
                Gamepad.current?.startButton.wasPressedThisFrame == true)
            {
                _canvasAudioController.Play(CanvasAudioController.CanvasAudioSoundType.Start);
                _canvasController.SwitchUI(TargetUiPageType, true);
            }
        }
    }
}
