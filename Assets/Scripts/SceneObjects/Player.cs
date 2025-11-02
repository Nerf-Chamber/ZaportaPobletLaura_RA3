using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private Vector2 onMoveDirection;

    private bool isRunning;
    private bool isFacingRight;

    private bool didCameraChangeOne = false;
    private bool canCameraChange = true;

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
    public void OnJump(InputAction.CallbackContext context)
    {
        if (GetIsGrounded())
        {
            _animation.FlipY();
            _jump.InvertGravity();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canCameraChange) return;

        // Eventually a switch for every change of stage trigger
        if (collision.gameObject.layer == LayerMask.NameToLayer("CameraChangeOne"))
        {
            CameraLocations locationToGo = didCameraChangeOne ? CameraLocations.StageOne : CameraLocations.StageTwo;
            CameraManager.Instance.ChangeStage(locationToGo);
            didCameraChangeOne = !didCameraChangeOne;

            // Coroutine: Special type of function that can be paused and resumed later. Concurrency and multitasking :D
            StartCoroutine(CameraChangeCooldown(0.5f));
        }
    }

    // IEnumerator type return for coroutine
    private IEnumerator CameraChangeCooldown(float delay)
    {
        canCameraChange = false;
        // Yield: Returns one value at a time
        yield return new WaitForSeconds(delay);
        canCameraChange = true;
    }

    private bool GetIsGrounded()
    {
        float raycastDistance = 0.7f;

        bool hitFloor = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, LayerMask.GetMask("Terrain"));
        bool hitCeiling = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, LayerMask.GetMask("Terrain"));

        return hitFloor || hitCeiling;
    }
}