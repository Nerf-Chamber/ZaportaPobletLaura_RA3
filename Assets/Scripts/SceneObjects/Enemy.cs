using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float timeRunning;
    [SerializeField] private float timeSleeping;

    public bool isSleeping;

    public void Start()
    {
        RandomJump();
        isSleeping = false;
    }

    // Cycle -> Sleep() -> onMoveDirection.x = isFacingRight ? 1 : -1; Run()

    private void RandomJump()
    {
        if (GetIsGrounded(0.7f) && !isSleeping)
        {
            int random = Random.Range(0, 1);

            if (random == 0) { _jump.Jump(150f); }
            else { _animation.FlipY(); _jump.InvertGravity(); }
        }

        float randomTimeToWait = Random.Range(5f, 10f);
        Invoke("RandomJump", randomTimeToWait);
    }
    private void Run()
    {
        isSleeping = false;
        if (GetDidHitHorizontal(0.7f, isFacingRight)) { onMoveDirection *= -1; }

        _move.MoveX(onMoveDirection.x, speed);
        if (!isFacingRight && onMoveDirection.x > 0) { _animation.FlipX(ref isFacingRight); }
        else if (isFacingRight && onMoveDirection.x < 0) { _animation.FlipX(ref isFacingRight); }
    }
    private void Sleep()
    {
        isSleeping = true;
        onMoveDirection = Vector2.zero;
        // Code animation hedgehog sleeping
    }
}