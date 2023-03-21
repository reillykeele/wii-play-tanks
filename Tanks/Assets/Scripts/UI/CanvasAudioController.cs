using System.Linq;
using Data.Audio;
using ScriptableObject.Audio;
using UnityEngine;
using Util.Audio;

public class CanvasAudioController : MonoBehaviour
{
    private AudioSource _backgroundAudioSource;
    private AudioSource _uiAudioSource;

    public AudioDataScriptableObject BackgroundAudioData;
    public LoopingMusicAudioDataScriptableObject BackgroundLoopingMusicAudioData;
    public AudioDataScriptableObject[] AudioData = new AudioDataScriptableObject[6];

    public enum CanvasAudioSoundType 
    {
        Start,
        Tick,
        Select,
        Back,
        Pause,
        Resume
    }


    void Awake()
    {
        _backgroundAudioSource = BackgroundLoopingMusicAudioData?.LoopingMusicAudioData.CreateNewAudioSource(gameObject) ??
                                 BackgroundAudioData?.AudioData.CreateNewAudioSource(gameObject);
        _uiAudioSource = AudioData.FirstOrDefault(x => x != null)?.AudioData.CreateNewAudioSource(gameObject);
    }

    void Start()
    {
        if (BackgroundLoopingMusicAudioData != null)
        {
            _backgroundAudioSource.Initialize(BackgroundLoopingMusicAudioData.LoopingMusicAudioData);
            _backgroundAudioSource.Play();
            StartCoroutine(_backgroundAudioSource.WaitForSound(() =>
            {
                _backgroundAudioSource.loop = true;
                _backgroundAudioSource.clip = BackgroundLoopingMusicAudioData.LoopingMusicAudioData.LoopAudioClip;
                _backgroundAudioSource.Play();
            }));
        }
        else if (BackgroundAudioData != null)
        {
            _backgroundAudioSource.Initialize(BackgroundAudioData.AudioData);
            _backgroundAudioSource.Play();
        }
    }

    public void Play(AudioClip audioClip)
    {
        if (audioClip != null)
            _uiAudioSource.PlayOneShot(audioClip);
    }

    public void Play(AudioData audioData) => Play(audioData?.AudioClip);

    public void Play(CanvasAudioSoundType audioSoundType) => Play(AudioData[(int) audioSoundType]?.AudioData);

    public void FadeOutBackgroundMusic()
    {
        if (_backgroundAudioSource != null)
            StartCoroutine(AudioHelper.FadeOut(_backgroundAudioSource, 1f));
    }
}
