using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, InputSystem_Actions.IPlayerActions
{
    private AudioClip clip;
    private InputSystem_Actions inputActions;
    private Dictionary<(CameraLocations, CameraLocations), bool> cameraChangeStates;
    private bool canCameraChange = true;
    private bool enteredFinalZone = false;

    public Vector2 initialPosition;

    public bool isDead = false;

    private bool canCollectCoins = true;
    public int coinsCollected = 0;

    override protected void Awake()
    {
        base.Awake();
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
        initialPosition = transform.position;
        isFacingRight = true;

        PauseMenu.OnPauseStateChanged += HandlePauseStateChanged;
        PauseMenu.OnRestartChosen += RestartPlayer;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Start()
    {
        cameraChangeStates = new Dictionary<(CameraLocations, CameraLocations), bool>
        {
            {(CameraLocations.StageOne, CameraLocations.StageTwo), false},
            {(CameraLocations.StageTwo, CameraLocations.StageThree), false},
            {(CameraLocations.StageThree, CameraLocations.StageFour), false},
            {(CameraLocations.StageFour, CameraLocations.StageOne), false}
        };
    }

    private void Update()
    {
        isMovingX = onMoveDirection.x != 0;

        _move.MoveX(onMoveDirection.x, speed);
        if (!isFacingRight && onMoveDirection.x > 0) { _animation.FlipX(ref isFacingRight); }
        else if (isFacingRight && onMoveDirection.x < 0) { _animation.FlipX(ref isFacingRight); }
        _animation.SetRunState(isMovingX);
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
        if (GetIsGrounded(0.7f))
        {
            _animation.FlipY();
            _jump.InvertGravity();
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Threat") && !isDead) { Loose(); }
        if (collision.gameObject.layer == LayerMask.NameToLayer("BouncyTerrain"))
        {
            if (AudioManager.Instance.clipList.TryGetValue(AudioClips.BouncySound, out clip))
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        CameraChangesCollision(collision);
        CollectableCollision(collision);

        if (collision.gameObject.layer == LayerMask.NameToLayer("FinalZone") && !enteredFinalZone)
        {
            enteredFinalZone = true;
            EnterFinalZone();
        }
    }

    private void EnterFinalZone()
    {
        if (coinsCollected == 8) { Win(); }
        else { Loose(); }
    }
    private void CameraChangesCollision(Collider2D collision)
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
            case int layer when layer == LayerMask.NameToLayer("CameraChangeThree"):
                locations = (CameraLocations.StageThree, CameraLocations.StageFour);
                break;
            case int layer when layer == LayerMask.NameToLayer("CameraChangeFour"):
                locations = (CameraLocations.StageFour, CameraLocations.StageOne);
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

    private void CollectableCollision(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<ICollectable>(out ICollectable iCollected) && canCollectCoins)
        {
            iCollected.Collected();
            if (iCollected is Coin coin)
            {
                coinsCollected++;
                canCollectCoins = false;
                Invoke(nameof(CanCollectAgain), 0.2f);
            }
        }
    }
    private void CanCollectAgain() { canCollectCoins = true; }

    private void Win() => Debug.Log("You won!");
    private void Loose() => Debug.Log("You died!");

    private void HandlePauseStateChanged(bool isPaused)
    {
        if (isPaused) inputActions.Player.Disable();
        else inputActions.Player.Enable();
    }
    private void RestartPlayer()
    {
        transform.position = initialPosition;
        if (transform.localScale.x < 0) { _animation.FlipX(ref isFacingRight); }
        coinsCollected = 0;
        ResetCameraValues();
    }
    private void ResetCameraValues()
    {
        var keys = new List<(CameraLocations, CameraLocations)>(cameraChangeStates.Keys);
        foreach (var key in keys) { cameraChangeStates[key] = false; }
    }
}