using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private Vector2 onMoveDirection;
    private bool isRunning;
    private bool isFacingRight;

    public float speed;

    override protected void Awake()
    {
        base.Awake();
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
        isFacingRight = transform.localScale.x == 1;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        isRunning = onMoveDirection.x != 0;

        if (isRunning)
        {
            _move.MoveX(onMoveDirection.x, speed);
            if (!isFacingRight && onMoveDirection.x > 0) { _animation.FlipX(ref isFacingRight); }
            else if (isFacingRight && onMoveDirection.x < 0) { _animation.FlipX(ref isFacingRight); }
        }
        _animation.SetRunState(isRunning);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.started)
        {
            onMoveDirection = context.ReadValue<Vector2>();
        } 
        else if (context.canceled)
        {
            onMoveDirection = Vector2.zero;
        }
    }
}