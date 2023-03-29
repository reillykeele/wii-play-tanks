using Data;
using Data.Enum;
using Manager;
using TMPro;
using Util.Helpers;

namespace UI.UIControllers
{
    public class HudUIController : UIController
    {
        private TextMeshProUGUI _missionText;
        private TextMeshProUGUI _tankCountText;

        protected override void Awake()
        {
            base.Awake();

            var levelClearBanner= transform.parent.GetChildObject("LevelTransition").GetChildObject("MissionBanner");
            _missionText = levelClearBanner.GetChildObject("MissionText").GetComponent<TextMeshProUGUI>();
            _tankCountText = levelClearBanner.GetChildObject("EnemyTankNumberText").GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            GameManager.Instance.OnPauseGameEvent.AddListener(PauseGame);
            GameManager.Instance.OnResumeGameEvent.AddListener(ResumeGame);
            GameManager.Instance.LevelClearEvent.AddListener(DisplayMissionClear);
            GameManager.Instance.TransitionLevelEvent.AddListener(DisplayMissionBanner);
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

        public void DisplayMissionClear()
        {
            _canvasController.SwitchUI(UIPageType.MissionClearScreen);
        }

        public void DisplayMissionBanner(LevelData levelData)
        {
            _missionText.text = $"Mission {levelData.LevelName}";
            _tankCountText.text = $"Enemy Tanks: {levelData.NumTanks}";

            _canvasController.SwitchUI(UIPageType.MissionScreen);
        }
    }
}
