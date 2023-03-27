using Data.Enum;
using Manager;

namespace UI.UIControllers
{
    public class HudUIController : UIController
    {

        protected override void Awake()
        {
            base.Awake();
        }

        void Start()
        {
            GameManager.Instance.OnPauseGameEvent.AddListener(PauseGame);
            GameManager.Instance.OnResumeGameEvent.AddListener(ResumeGame);
            GameManager.Instance.LevelClearEvent.AddListener(DisplayLevelClear);
            GameManager.Instance.TransitionLevelEvent.AddListener(DisplayLevelTransition);
        }

        void Update()
        {
            
        }

        public void PauseGame()
        {
            // _canvasAudioController.Play(CanvasAudioController.CanvasAudioSoundType.Pause);
            _canvasController.DisplayUI(UIPageType.PauseMenu);
        }

        public void ResumeGame()
        {
            
        }

        public void DisplayLevelClear()
        {
            _canvasController.SwitchUI(UIPageType.MissionClearScreen);
        }

        public void DisplayLevelTransition()
        {
            _canvasController.SwitchUI(UIPageType.MissionScreen);
        }
    }
}
