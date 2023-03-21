using Data.Audio;
using UnityEngine;

namespace ScriptableObject.Audio
{
    [CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/Audio/Looping Music Audio Data Clip")]
    public class LoopingMusicAudioDataScriptableObject : UnityEngine.ScriptableObject
    {
        public LoopingMusicAudioData LoopingMusicAudioData;
    }
}