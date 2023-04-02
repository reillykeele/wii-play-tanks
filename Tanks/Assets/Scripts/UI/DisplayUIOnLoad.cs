using Manager;
using Scene;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(UIController))]
    public class DisplayUIOnLoad : AOnSceneLoad
    {
        private UIController _uiController;

        void Awake()
        {
            _uiController = GetComponent<UIController>();
        }

        protected override void Start()
        {
            if (LoadingManager.Instance.IsLoading)
                _uiController.Disable();
        }

        protected override void OnSceneLoad()
        {
            _uiController.Enable();
        }
    }
}
