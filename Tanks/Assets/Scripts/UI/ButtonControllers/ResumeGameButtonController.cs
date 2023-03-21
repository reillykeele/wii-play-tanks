using Manager;

namespace UI.ButtonControllers
{
    public class ResumeGameButtonController : AButtonController
    {
        public override void OnClick() => GameManager.Instance.ResumeGame();
    }
}
