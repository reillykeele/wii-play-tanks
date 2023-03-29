using UnityEngine;
using Util.Helpers;

namespace UI.UIElementControllers
{
    public class TimedDisableUIElementController : MonoBehaviour
    {
        public bool DisableOnAwake = true;
        public float TimeBeforeTransition = 1f;

        private bool _isInitialized = false;

        void Awake()
        {
            if (DisableOnAwake)
                gameObject.Disable();
        }

        public void OnEnable()
        {
            // Do not run when we are first initialized, only when we are turned on
            if (_isInitialized == false)
            {
                _isInitialized = true;
                return;
            }

            Invoke("Disable", TimeBeforeTransition);
        }

        private void Disable() => gameObject.Disable();
    }
}
