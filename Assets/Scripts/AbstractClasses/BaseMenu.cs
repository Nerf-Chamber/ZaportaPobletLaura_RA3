using UnityEngine;
using System;

public abstract class BaseMenu : MonoBehaviour
{
    public GameObject MenuUI;

    public static event Action<bool> OnPauseStateChanged;
    public static event Action OnRestartChosen;

    protected bool isActive;

    protected virtual void Awake() => MenuUI.SetActive(false);

    protected virtual void ShowMenu()
    {
        MenuUI.SetActive(true);
        Time.timeScale = 0f;
        isActive = true;
        OnPauseStateChanged.Invoke(true);
    }

    // Public because is accessed by the Resume button of PauseMenu
    public void HideMenu()
    {
        MenuUI.SetActive(false);
        Time.timeScale = 1f;
        isActive = false;
        OnPauseStateChanged.Invoke(false);
    }

    public void ExitGame() => Application.Quit();

    public virtual void Restart()
    {
        OnRestartChosen.Invoke();
        HideMenu();
    }
}