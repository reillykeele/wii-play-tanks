using Manager;
using Util.Enums;

namespace UI.ButtonControllers
{
    public class SetGameStateButtonController : AButtonController
    {
        public GameState TargetGameState;
    
        public override void OnClick()
        {
            GameManager.Instance.CurrentGameState = TargetGameState;
        }
    }
}