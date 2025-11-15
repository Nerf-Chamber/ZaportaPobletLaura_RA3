using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour, InputSystem_Actions.IUIActions
{
    private InputSystem_Actions inputActions;

    public static bool GamePaused = false;
    public GameObject PauseMenuUI;

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

    private void Resume() 
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    private void Pause() 
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GamePaused = true;
    }
}
