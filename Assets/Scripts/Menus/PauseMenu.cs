using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, InputSystem_Actions.IUIActions
{
    private InputSystem_Actions inputActions;

    public static bool GamePaused = false;
    public GameObject PauseMenuUI;

    public static event Action<bool> OnPauseStateChanged;
    public static event Action OnRestartChosen;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.UI.SetCallbacks(this);
        PauseMenuUI.SetActive(false);
    }
    private void OnEnable() => inputActions.UI.Enable();
    private void OnDisable() => inputActions.UI.Disable();

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (GamePaused) Resume();
        else Pause();
    }

    public void Resume() 
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
        OnPauseStateChanged?.Invoke(false);
    }
    public void Exit() { Application.Quit(); }
    public void Restart() 
    { 
        OnRestartChosen.Invoke();
        Resume();
    }

    private void Pause() 
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
        OnPauseStateChanged.Invoke(true);
    }
}
