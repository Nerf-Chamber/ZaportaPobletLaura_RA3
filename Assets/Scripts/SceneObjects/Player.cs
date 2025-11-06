using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    private Vector2 onMoveDirection;

    private Dictionary<(CameraLocations, CameraLocations), bool> cameraChangeStates;

    private bool isRunning;
    private bool isFacingRight;
    private bool canCameraChange = true;

    public bool isDead = false;

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

    private void Start()
    {
        cameraChangeStates = new Dictionary<(CameraLocations, CameraLocations), bool>
        {
            {(CameraLocations.StageOne, CameraLocations.StageTwo), false},
            {(CameraLocations.StageTwo, CameraLocations.StageThree), false}
        };
    }

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
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Threat") && !isDead)
        {
            isDead = true;
            _animation.SetDeathState(true);
            _jump.Jump(175f);
            inputActions.Disable();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canCameraChange) return;

        (CameraLocations, CameraLocations) locations;

        switch (collision.gameObject.layer)
        {
            case int layer when layer == LayerMask.NameToLayer("CameraChangeOne"):
                locations = (CameraLocations.StageOne, CameraLocations.StageTwo);
                break;
            case int layer when layer == LayerMask.NameToLayer("CameraChangeTwo"):
                locations = (CameraLocations.StageTwo, CameraLocations.StageThree);
                break;
            default:
                return;
        }

        if (cameraChangeStates.TryGetValue(locations, out bool didCameraChange))
        {
            ChangeCamera(locations, ref didCameraChange);
            cameraChangeStates[locations] = didCameraChange;
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
    private void ChangeCamera((CameraLocations, CameraLocations) locations, ref bool didCameraChange)
    {
        CameraLocations locationToGo = didCameraChange ? locations.Item1 : locations.Item2;
        CameraManager.Instance.ChangeStage(locationToGo);
        didCameraChange = !didCameraChange;

        // Coroutine: Special type of function that can be paused and resumed later. Concurrency and multitasking :D
        StartCoroutine(CameraChangeCooldown(0.5f));
    }
    private bool GetIsGrounded()
    {
        float raycastDistance = 0.7f;

        bool hitFloor = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, LayerMask.GetMask("Terrain"));
        bool hitCeiling = Physics2D.Raycast(transform.position, Vector2.up, raycastDistance, LayerMask.GetMask("Terrain"));

        return hitFloor || hitCeiling;
    }
}