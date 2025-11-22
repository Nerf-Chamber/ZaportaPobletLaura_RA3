using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character, InputSystem_Actions.IPlayerActions
{
    public const int coinsToCollect = 8;

    private AudioClip clip;
    private InputSystem_Actions inputActions;

    public Vector2 initialPosition;

    public static bool isDead = false;
    public static bool isDeadAsASoup = false;
    public static bool didWin = false;
    public bool didReachTheEnd = false;

    public float limitChangeCameraX;
    public float limitChangeCameraY;

    private bool canCollectCoins = true;
    public int coinsCollected = 0;

    private AudioSource audioSource;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    public static event Action OnIntroDialogueTriggered;
    public static event Action OnEndDialogueTriggered;

    override protected void Awake()
    {
        base.Awake();
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
        initialPosition = transform.position;
        isFacingRight = true;

        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        BaseMenu.OnPauseStateChanged += HandlePauseStateChanged;
        BaseMenu.OnRestartChosen += RestartPlayer;
        Dialogue.OnIntroDialogueEnded += AllowMovement;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        // Movement related
        isMovingX = onMoveDirection.x != 0;

        _move.MoveX(onMoveDirection.x, speed);
        if (!isFacingRight && onMoveDirection.x > 0) { _animation.FlipX(ref isFacingRight); }
        else if (isFacingRight && onMoveDirection.x < 0) { _animation.FlipX(ref isFacingRight); }
        _animation.SetRunState(isMovingX);

        // Camera related
        if (transform.position.x > limitChangeCameraX)
        {
            if (transform.position.y > limitChangeCameraY) CameraManager.Instance.ChangeStage(CameraLocations.StageFour);
            else CameraManager.Instance.ChangeStage(CameraLocations.StageThree);
        }
        else
        {
            if (transform.position.y > limitChangeCameraY) CameraManager.Instance.ChangeStage(CameraLocations.StageOne);
            else CameraManager.Instance.ChangeStage(CameraLocations.StageTwo);
        }
    }

    // ----- MOVEMENT -----
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.started) { onMoveDirection = context.ReadValue<Vector2>(); }
        else if (context.canceled) { onMoveDirection = Vector2.zero; }
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
            AudioManager.PlaySound(audioSource, clip, AudioClips.BouncySound);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        CollectableCollision(collision);
        DialogueCollision(collision);
    }

    private void DialogueCollision(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("IntroDialogue") && !Dialogue.hasIntroHappened)
        {
            inputActions.Disable();
            OnIntroDialogueTriggered.Invoke();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("EndDialogue") && !didReachTheEnd)
        {
            didReachTheEnd = true;

            if (coinsCollected < coinsToCollect) LooseAsASoup();
            else Win();

            inputActions.Disable();
            OnEndDialogueTriggered.Invoke();
        }
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

    private void Win() => didWin = true;
    private void Loose()
    {
        inputActions.Disable();
        boxCollider.enabled = false;
        rigidBody.bodyType = RigidbodyType2D.Static;
        AudioManager.PlaySound(audioSource, clip, AudioClips.DeadSound);
        _animation.SetDeadState(true);

    }
    private void LooseAsASoup()
    {
        isDeadAsASoup = true;

        inputActions.Disable();
        _jump.Jump(750f);
        _animation.SetDeadSoupState(true);
        // Quan s'acabés el diàleg de la poma game over xd
    }
    private void ActivateGameOverMenu()
    {
        spriteRenderer.enabled = false;
        StartCoroutine(DeadDelay(0.2f));
    }
    private IEnumerator DeadDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isDead = true;
    }

    // ----- DELEGATES -----
    private void HandlePauseStateChanged(bool isPaused)
    {
        if (isPaused) inputActions.Player.Disable();
        else inputActions.Player.Enable();
    }
    private void RestartPlayer()
    {
        inputActions.Enable();

        isDead = false;
        didWin = false;

        boxCollider.enabled = true;
        spriteRenderer.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;

        _animation.SetDeadState(false);
        _animation.SetDeadSoupState(false);

        if (transform.localScale.y < 0)
        {
            _jump.InvertGravity();
            _animation.FlipY();
        }
        transform.position = initialPosition;
        if (transform.localScale.x < 0) { _animation.FlipX(ref isFacingRight); }

        coinsCollected = 0;
        ResetCameraValues();
    }
    private void ResetCameraValues() => CameraManager.Instance.ChangeStage(CameraLocations.StageOne);
    private void AllowMovement() { if (!didWin && !isDead) { inputActions.Enable(); } }
}