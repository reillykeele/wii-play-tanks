using Data.Enum;
using Util.Enums;

namespace UI.ButtonControllers
{
    public class ChangeUIPageButtonController : AButtonController
    {
        public UIPageType TargetUiPageType;
        public bool FadeIn = false;
        public bool FadeOut = false;
    
        public override void OnClick()
        {
            _canvasController.SwitchUI(TargetUiPageType);
        }
    }
}