using System.Collections.Generic;
using UnityEngine;

public enum AudioClips 
{ 
    MainTheme
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
    }
}