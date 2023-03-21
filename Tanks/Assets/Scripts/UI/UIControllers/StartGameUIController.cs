using Util.Enums;

namespace UI.UIControllers
{
    public class StartGameUIController : UIController
    {
        public SceneType TargetScene = SceneType.Game;

        public void OnClick()
        {
            _canvasController.SwitchScene(TargetScene);
        }
    }
}
