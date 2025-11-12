using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioClip clip = null;

    private void Start()
    {
        if (AudioManager.Instance.clipList.TryGetValue(AudioClips.MainTheme, out clip))
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}