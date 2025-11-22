using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameOverMenu : BaseMenu
{
    [SerializeField] private TextMeshProUGUI GameOverText;
    [SerializeField] private TextMeshProUGUI GameOverStateText;

    private AudioSource audioSource;
    private AudioClip clip;

    private const string win = "Congrats! You won :D";
    private const string loose = "Nooooo, you lost :c";

    // Garanteix que el menú només s'obre una vegada
    private bool wasNotDead = false;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        Apple.OnAppleFaceEnded += ShowMenuForApple;
    }

    private void Update()
    {
        if (Player.isDead && !wasNotDead)
        {
            wasNotDead = true;
            ShowMenu();
        }
    }
    public override void Restart()
    {
        wasNotDead = false;     
        base.Restart();      
    }
    protected override void ShowMenu()
    {
        base.ShowMenu();
        if (Player.isDead || Player.isDeadAsASoup)
        {
            AudioManager.PlaySound(audioSource, clip, AudioClips.LooseSound);
            GameOverText.color = Color.red;
            GameOverStateText.text = loose;
        } 
        else
        {
            // Aquí tampoc xd
            AudioManager.PlaySound(audioSource, clip, AudioClips.WinSound);
            GameOverText.color = Color.green;
            GameOverStateText.text = win;
        }
    }
    private void ShowMenuForApple() => ShowMenu();
}