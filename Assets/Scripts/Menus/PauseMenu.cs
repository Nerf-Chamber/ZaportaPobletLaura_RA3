using UnityEngine.InputSystem;

public class PauseMenu : BaseMenu, InputSystem_Actions.IUIActions
{
    private InputSystem_Actions inputActions;

    protected override void Awake()
    {
        base.Awake();
        inputActions = new InputSystem_Actions();
        inputActions.UI.SetCallbacks(this);
    }

    private void OnEnable() => inputActions.UI.Enable();
    private void OnDisable() => inputActions.UI.Disable();

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isActive) HideMenu();
        else ShowMenu();
    }
}