using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioClips 
{ 
    MainTheme,
    CoinSound,
    BouncySound,
    DeadSound
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    public Dictionary<AudioClips, AudioClip> clipList = new Dictionary<AudioClips, AudioClip> { };

    private void Awake()
    {
        Instance = this;
        clipList.Add(AudioClips.MainTheme, audioClips[0]);
        clipList.Add(AudioClips.CoinSound, audioClips[1]);
        clipList.Add(AudioClips.BouncySound, audioClips[2]);
        clipList.Add(AudioClips.DeadSound, audioClips[3]);
    }

    public static void PlaySound(AudioSource audioSource, AudioClip clip, AudioClips sound)
    {
        if (Instance.clipList.TryGetValue(sound, out clip))
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}