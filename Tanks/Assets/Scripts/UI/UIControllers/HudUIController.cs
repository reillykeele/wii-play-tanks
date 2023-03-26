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
    }
}
