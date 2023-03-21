using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Data.Audio
{
    [Serializable]
    public class MusicAudioData
    {
        public AudioClip AudioClip;
        public AudioMixerGroup MixingGroup;

        public float loopStartTime = 0f;

        [Range(0f, 1f)]
        public float Volume = 1f;

        public bool PlayOnAwake = false;
        public bool Loop = false;
        // public float FadeTime = 0f;
    }

    public static class MusicAudioDataExtension
    {
        // public static AudioSource Initialize(this AudioSource audioSource, AudioData audioData)
        // {
        //     audioSource.clip = audioData.AudioClip;
        //     audioSource.outputAudioMixerGroup = audioData.MixingGroup;
        //     audioSource.volume = audioData.Volume;
        //     audioSource.playOnAwake = audioData.PlayOnAwake;
        //     audioSource.loop = audioData.Loop;
        //
        //     return audioSource;
        // }
        //
        // public static AudioSource CreateNewAudioSource(this AudioData audioData, GameObject gameObject)
        // {
        //     return gameObject.AddComponent<AudioSource>().Initialize(audioData);
        // }
    }
}