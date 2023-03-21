using Data.Audio;
using UnityEngine;

namespace ScriptableObject.Audio
{
    [CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/Audio/Audio Data Clip")]
    public class AudioDataScriptableObject : UnityEngine.ScriptableObject
    {
        public AudioData AudioData;
    }
}