using Data;
using Manager;
using UnityEngine;

namespace UI.ButtonControllers
{
    public class LoadLevelDataButtonController : AButtonController
    {
        public LevelData LevelData;

        public override void OnClick()
        {
            _canvasAudioController?.FadeOutBackgroundMusic();
            GameManager.Instance.LoadLevel(LevelData);
        }
    }
}
