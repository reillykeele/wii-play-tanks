using Data.Enum;
using Util.Enums;

namespace UI.UIControllers
{
    public class TimedTransitionUIController : UIController
    {
        public UIPageType TargetUI;
        public float TimeBeforeTransition = 1f;

        public void OnEnable()
        {
            Invoke("Transition", TimeBeforeTransition);
        }

        private void Transition() => _canvasController.SwitchUI(TargetUI);
    }
}
