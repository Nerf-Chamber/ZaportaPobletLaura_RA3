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
        AudioManager.PlaySound(audioSource, clip, AudioClips.MainMenuMiniTheme);
    }

    private void HandlePauseState(bool isPaused)
    {
        if (isPaused && (!Player.isDead || !Player.didWin)) audioSource.Pause();
        else audioSource.UnPause();
    }
    private void HandleRestart()
    {
        if (!MainMenu.GameStarted) 
        {
            audioSource.loop = true;
            AudioManager.PlaySound(audioSource, clip, AudioClips.MainTheme); 
            MainMenu.GameStarted = true;
        }
        audioSource.Stop();
        audioSource.time = 0f;
        audioSource.Play();
    }
}