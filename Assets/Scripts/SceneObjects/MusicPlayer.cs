using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake() { audioSource = GetComponent<AudioSource>(); }

    private void OnEnable() { PauseMenu.OnPauseStateChanged += HandlePauseState; }
    private void OnDisable() { PauseMenu.OnPauseStateChanged -= HandlePauseState; }

    private void Start()
    {
        if (AudioManager.Instance.clipList.TryGetValue(AudioClips.MainTheme, out AudioClip clip))
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void HandlePauseState(bool isPaused)
    {
        if (isPaused) audioSource.Pause();
        else audioSource.UnPause();
    }
}