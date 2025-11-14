using System.Collections.Generic;
using UnityEngine;

public enum AudioClips 
{ 
    MainTheme,
    CoinSound,
    BouncySound
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
    }
}