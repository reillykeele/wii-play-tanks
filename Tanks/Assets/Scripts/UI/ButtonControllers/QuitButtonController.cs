using Manager;

namespace UI.ButtonControllers
{
    public class QuitButtonController : AButtonController
    {
        public override void OnClick()
        {
            _canvasAudioController?.FadeOutBackgroundMusic();
            LoadingManager.Instance.QuitGame();
        }
    }
}