using UnityEngine;

namespace Util.Helpers
{
    public class DontDestroyOnLoadBehaviour : MonoBehaviour
    {
        void Awake() => DontDestroyOnLoad(this);
    }
}