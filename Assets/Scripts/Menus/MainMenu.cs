using UnityEngine;

public class MainMenu : BaseMenu
{
    public static bool GameStarted = false;

    protected override void Awake() => MenuUI.SetActive(true);
}