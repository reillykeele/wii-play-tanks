using Manager;
using Scene;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(UIController))]
    public class DisplayUIOnLoad : AOnSceneLoad
    {
        private UIController _uiController;

        protected override void Awake()
        {
            base.Awake();

            _uiController = GetComponent<UIController>();
            
            if (LoadingManager.Instance.IsLoading)
                _uiController.Disable();
        }

        protected override void OnSceneLoad()
        {
            _uiController.Enable();
        }
    }
}
