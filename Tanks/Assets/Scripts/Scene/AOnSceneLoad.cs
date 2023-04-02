using Manager;
using UnityEngine;

namespace Scene
{
    public abstract class AOnSceneLoad : MonoBehaviour
    {
        protected virtual void Start()
        {
            LoadingManager.Instance.OnSceneLoadedEvent.AddListener(OnSceneLoad);
        }

        protected abstract void OnSceneLoad();
    }
}
