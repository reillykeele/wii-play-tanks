using Data.Enum;
using UnityEngine;
using Util.Enums;

namespace UI.UIControllers
{
    public class TimedTransitionUIController : UIController
    {
        public UIPageType TargetUI;
        public float TimeBeforeTransition = 1f;

        private bool _isInitialized = false;

        public void OnEnable()
        {
            // Do not run when we are first initialized, only when we are turned on
            if (_isInitialized == false)
            {
                _isInitialized = true;
                return;
            }

            Invoke("Transition", TimeBeforeTransition);
        }

        private void Transition() => _canvasController.SwitchUI(TargetUI);
    }
}
