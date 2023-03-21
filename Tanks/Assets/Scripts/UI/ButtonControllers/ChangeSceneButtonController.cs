using Util.Coroutine;
using Util.Enums;

namespace UI.ButtonControllers
{
    public class ChangeSceneButtonController : AButtonController
    {
        public SceneType TargetScene;
        public float Delay = 0f;
    
        public override void OnClick()
        {
            _canvasAudioController?.FadeOutBackgroundMusic();
            StartCoroutine(CoroutineUtil.WaitForExecute(() => _canvasController.SwitchScene(TargetScene), Delay));
        }
    }
}
