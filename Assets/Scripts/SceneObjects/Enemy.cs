using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float timeRunning = 2f;
    [SerializeField] private float timeSleeping = 2f;

    private bool isSleeping;
    private float currentDirection = 1f;

    override protected void Awake()
    {
        base.Awake();
        isFacingRight = transform.localScale.x == 1;
    }

    private void Start()
    {
        // Sleep - Run Cycle
        Sleep();
        RandomJump();
    }

    private void Update()
    {
        _move.MoveX(currentDirection, speed);
        if (GetDidHitHorizontal(0.7f, isFacingRight)) { currentDirection *= -1; }

        if (!isFacingRight && currentDirection > 0) { _animation.FlipX(ref isFacingRight); }
        else if (isFacingRight && currentDirection < 0) { _animation.FlipX(ref isFacingRight); }
    }

    private void RandomJump()
    {
        if (GetIsGrounded(0.7f) && !isSleeping)
        {
            _jump.Jump(150f);
        }

        float randomTimeToWait = Random.Range(1f, 4f);
        Invoke(nameof(RandomJump), randomTimeToWait);
    }

    private void Run()
    {
        isSleeping = false;
        currentDirection = isFacingRight ? 1f : -1f;
        _animation.SetSleepState(isSleeping);

        Invoke(nameof(Sleep), timeRunning);
    }

    private void Sleep()
    {
        isSleeping = true;
        currentDirection = 0;
        _animation.SetSleepState(isSleeping);

        Invoke(nameof(Run), timeSleeping);
    }
}