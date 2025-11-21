using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip clip;

    private void Awake() { audioSource = GetComponent<AudioSource>(); }

    private void OnEnable()
    {
        BaseMenu.OnPauseStateChanged += HandlePauseState;
        BaseMenu.OnRestartChosen += HandleRestart;
    }
    private void OnDisable()
    {
        BaseMenu.OnPauseStateChanged -= HandlePauseState;
        BaseMenu.OnRestartChosen -= HandleRestart;
    }

    private void Start()
    {
        AudioManager.PlaySound(audioSource, clip, AudioClips.MainTheme);
    }

    private void HandlePauseState(bool isPaused)
    {
        if (isPaused) audioSource.Pause();
        else audioSource.UnPause();
    }
    private void HandleRestart()
    {
        audioSource.Stop();
        audioSource.time = 0f;
        audioSource.Play();
    }
}