using UnityEngine;

public class MainMenu : BaseMenu
{
    public static bool GameStarted = false;

    protected override void Awake() => MenuUI.SetActive(true);

    // Works as the start
    public override void Restart()
    {
        base.Restart();
        GameStarted = true;
    }
}